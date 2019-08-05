﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Csg
{
    public class Plane : IEquatable<Plane>
    {
        const float EPSILON = 1e-5f;

        public readonly Vector3 Normal;
        public readonly float W;
        int tag = 0;
        public int Tag
        {
            get
            {
                if (tag == 0)
                {
                    tag = Solid.GetTag();
                }
                return tag;
            }
        }
        public Plane(Vector3 normal, float w)
        {
            Normal = normal;
            W = w;
        }
        public bool Equals(Plane n)
        {
#pragma warning disable RECS0018 // Comparison of floating point numbers with equality operator
            return Normal.Equals(n.Normal) && W == n.W;
#pragma warning restore RECS0018 // Comparison of floating point numbers with equality operator
        }
        public Plane Flipped()
        {
            return new Plane(Normal * -1, -W);
        }
        public unsafe void SplitPolygon(Polygon polygon, out SplitPolygonResult result)
        {
            result = new SplitPolygonResult();
            var planenormal = this.Normal;
            var vertices = polygon.Vertices;
            var numvertices = vertices.Count;
            if (polygon.Plane.Equals(this))
            {
                result.Type = 0;
            }
            else
            {
                var EPS = Plane.EPSILON;
                var thisw = this.W;
                var hasfront = false;
                var hasback = false;
                var vertexIsBack = stackalloc bool[numvertices];
                var MINEPS = -EPS;
                for (var i = 0; i < numvertices; i++)
                {
                    var t = Vector3.Dot(planenormal, vertices[i].Pos) - thisw;
                    var isback = (t < 0);
                    vertexIsBack[i] = isback;
                    if (t > EPS) hasfront = true;
                    if (t < MINEPS) hasback = true;
                }
                if ((!hasfront) && (!hasback))
                {
                    // all points coplanar
                    var t = Vector3.Dot(planenormal, polygon.Plane.Normal);
                    result.Type = (t >= 0) ? 0 : 1;
                }
                else if (!hasback)
                {
                    result.Type = 2;
                }
                else if (!hasfront)
                {
                    result.Type = 3;
                }
                else
                {
                    // spanning
                    result.Type = 4;
                    var frontvertices = new List<Vertex>();
                    var backvertices = new List<Vertex>();
                    var isback = vertexIsBack[0];
                    for (var vertexindex = 0; vertexindex < numvertices; vertexindex++)
                    {
                        var vertex = vertices[vertexindex];
                        var nextvertexindex = vertexindex + 1;
                        if (nextvertexindex >= numvertices) nextvertexindex = 0;
                        var nextisback = vertexIsBack[nextvertexindex];
                        if (isback == nextisback)
                        {
                            // line segment is on one side of the plane:
                            if (isback)
                            {
                                backvertices.Add(vertex);
                            }
                            else
                            {
                                frontvertices.Add(vertex);
                            }
                        }
                        else
                        {
                            // line segment intersects plane:
                            var intersectionvertex = SplitLineBetweenVertices(vertex, vertices[nextvertexindex]);
                            if (isback)
                            {
                                backvertices.Add(vertex);
                                backvertices.Add(intersectionvertex);
                                frontvertices.Add(intersectionvertex);
                            }
                            else
                            {
                                frontvertices.Add(vertex);
                                frontvertices.Add(intersectionvertex);
                                backvertices.Add(intersectionvertex);
                            }
                        }
                        isback = nextisback;
                    } // for vertexindex
                      // remove duplicate vertices:
                    var EPS_SQUARED = Plane.EPSILON * Plane.EPSILON;
                    if (backvertices.Count >= 3)
                    {
                        var prevvertex = backvertices[backvertices.Count - 1];
                        for (var vertexindex = 0; vertexindex < backvertices.Count; vertexindex++)
                        {
                            var vertex = backvertices[vertexindex];
                            if (vertex.Pos.DistanceToSquared(prevvertex.Pos) < EPS_SQUARED)
                            {
                                backvertices.RemoveAt(vertexindex);
                                vertexindex--;
                            }
                            prevvertex = vertex;
                        }
                    }
                    if (frontvertices.Count >= 3)
                    {
                        var prevvertex = frontvertices[frontvertices.Count - 1];
                        for (var vertexindex = 0; vertexindex < frontvertices.Count; vertexindex++)
                        {
                            var vertex = frontvertices[vertexindex];
                            if (vertex.Pos.DistanceToSquared(prevvertex.Pos) < EPS_SQUARED)
                            {
                                frontvertices.RemoveAt(vertexindex);
                                vertexindex--;
                            }
                            prevvertex = vertex;
                        }
                    }
                    if (frontvertices.Count >= 3)
                    {
                        result.Front = new Polygon(frontvertices, polygon.Shared, polygon.Plane);
                    }
                    if (backvertices.Count >= 3)
                    {
                        result.Back = new Polygon(backvertices, polygon.Shared, polygon.Plane);
                    }
                }
            }
        }
        Vertex SplitLineBetweenVertices(Vertex v1, Vertex v2)
        {
            var p1 = v1.Pos;
            var p2 = v2.Pos;
            var direction = p2 - (p1);
            var u = (W - Vector3.Dot(Normal,p1)) / Vector3.Dot(Normal,direction);
            if (float.IsNaN(u)) u = 0;
            if (u > 1) u = 1;
            if (u < 0) u = 0;
            var result = p1 + (direction * (u));
            var tresult = v1.Tex + (v2.Tex - v1.Tex) * u;
            return new Vertex(result, tresult);
        }
        public static Plane FromVector3Ds(Vector3 a, Vector3 b, Vector3 c)
        {
            var n = Vector3.Cross(b - a,c - a).normalized;
            return new Plane(n, Vector3.Dot(n,a));
        }
        public Plane Transform(Matrix4x4 matrix4x4)
        {
            // get two vectors in the plane:
            var r = this.Normal.RandomNonParallelVector();
            var u = Vector3.Cross(this.Normal, r);
            var v = Vector3.Cross(this.Normal, u);
            // get 3 points in the plane:
            var point1 = matrix4x4.MultiplyPoint(this.Normal * this.W);
            var point2 = matrix4x4.MultiplyPoint(point1 + u);
            var point3 = matrix4x4.MultiplyPoint(point1 + v);
            // and create a new plane from the transformed points:
            var newplane = Plane.FromVector3Ds(point1, point2, point3);
            return newplane;
        }
    }

	public struct SplitPolygonResult
	{
		public int Type;
		public Polygon Front;
		public Polygon Back;
	}
}

