﻿using System;
using System.Collections.Generic;
using UnityEngine;
#nullable enable
namespace Csg
{
	/// <summary>
	/// Convex polygons comprised of vertices lying on a plane.
	/// Each polygon also has "Shared" data which is any
	/// metadata (usually a material reference) that you need to
	/// share between sets of polygons.
	/// </summary>
	public class Polygon
	{
		public readonly IReadOnlyList<Vertex> Vertices;
		public readonly Plane Plane;
		public readonly PolygonShared Shared;

		readonly bool debug = false;

		static readonly PolygonShared defaultShared = new PolygonShared(null);

		BoundingSphere? cachedBoundingSphere;
		Bounds? cachedBoundingBox;

		public Polygon(List<Vertex> vertices, PolygonShared? shared = null, Plane? plane = null)
		{
			Vertices = vertices;
			Shared = shared ?? defaultShared;
			Plane = plane ?? Plane.FromVector3Ds(vertices[0].Pos, vertices[1].Pos, vertices[2].Pos);
			if (debug)
			{
				//CheckIfConvex();
			}
		}

		public Polygon(params Vertex[] vertices)
			: this(new List<Vertex>(vertices))
		{
		}

		public BoundingSphere BoundingSphere
		{
			get
			{
				if (cachedBoundingSphere == null)
				{
					var box = BoundingBox;
					var middle = (box.min + box.max) * 0.5f;
					var radius3 = box.max - middle;
					var radius = radius3.magnitude;
					cachedBoundingSphere = new BoundingSphere { Center = middle, Radius = radius };
				}
				return cachedBoundingSphere;
			}
		}

		public Bounds BoundingBox
		{
			get
			{
				if (cachedBoundingBox == null)
				{
					Vector3 minpoint, maxpoint;
					var vertices = this.Vertices;
					var numvertices = vertices.Count;
					if (numvertices == 0)
					{
						minpoint = new Vector3(0, 0, 0);
					}
					else {
						minpoint = vertices[0].Pos;
					}
					maxpoint = minpoint;
					for (var i = 1; i < numvertices; i++)
					{
						var point = vertices[i].Pos;
						minpoint = Vector3.Min(minpoint, point);
						maxpoint = Vector3.Max(maxpoint, point);
					}
                    Vector3 size = maxpoint - minpoint;
                    Vector3 center = (minpoint + maxpoint) / 2;
                    cachedBoundingBox = new Bounds(center, size);
				}
                return (Bounds)cachedBoundingBox;
			}
		}

		public Polygon Flipped()
		{
			var newvertices = new List<Vertex>(Vertices.Count);
			for (int i = 0; i < Vertices.Count; i++)
			{
				newvertices.Add(Vertices[i].Flipped());
			}
			newvertices.Reverse();
			var newplane = Plane.Flipped();
			return new Polygon(newvertices, Shared, newplane);
		}

        // Are any vertices incommon
        public bool IsTouching(Polygon right) {
            if (!BoundingBox.Intersects(right.BoundingBox)) {
                return false;
            }

            for(int i=0; i < Vertices.Count; i++) {
                for (int j = 0; j < right.Vertices.Count; j++) {
					if(Vertices[i].Pos.Equals(right.Vertices[j].Pos)) {
                        return true;
                    }
                }
            }
            return false;
        }
    }

	public class PolygonShared
	{
		int tag = 0;
		public int Tag {
			get {
				if (tag == 0) {
					tag = Solid.GetTag ();
				}
				return tag;
			}
		}
		public PolygonShared(object? color)
		{			
		}
		public string Hash
		{
			get
			{
				return "null";
			}
		}
	}

	public class Properties
	{
		public readonly Dictionary<string, object> All = new Dictionary<string, object>();
		public Properties Merge(Properties otherproperties)
		{
			var result = new Properties();
			foreach (var x in All)
			{
				result.All.Add(x.Key, x.Value);
			}
			foreach (var x in otherproperties.All)
			{
				result.All[x.Key] = x.Value;
			}
			return result;
		}
		public Properties Transform(Matrix4x4 matrix4x4)
		{
			var result = new Properties();
			foreach (var x in All)
			{
				result.All.Add(x.Key, x.Value);
			}
			return result;
		}
	}
}

