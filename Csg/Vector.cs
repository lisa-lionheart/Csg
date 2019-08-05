using System;
using UnityEngine;

namespace Csg
{

    public static class Vector3Extensions {
        public static Vector3 Abs(this Vector3 self) {
                return new Vector3(Mathf.Abs(self.x), Mathf.Abs(self.y), Mathf.Abs(self.z));
        }

        public static Vector3 RandomNonParallelVector(this Vector3 self) {
            var abs = self.Abs();
            if ((abs.x <= abs.y) && (abs.x <= abs.z)) {
                return new Vector3(1, 0, 0);
            } else if ((abs.y <= abs.x) && (abs.y <= abs.z)) {
                return new Vector3(0, 1, 0);
            } else {
                return new Vector3(0, 0, 1);
            }
        }


        public static float DistanceToSquared(this Vector3 rhs, Vector3 a) {
            var dx = rhs.x - a.x;
            var dy = rhs.y - a.y;
            var dz = rhs.z - a.z;
            return dx * dx + dy * dy + dz * dz;
        }
    }

	public static class Vector2Extensions
    {

        public static float DistanceTo(this Vector2 self, Vector2 a) {
            var dx = self.x - a.x;
            var dy = self.y - a.y;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        public static float Dot(this Vector2 self, Vector2 a) {
            return self.x * a.x + self.y * a.y;
        }
        public static Vector2 Normal(this Vector2 self) {
            return new Vector2(self.y, -self.x);
        }
    }

	public class BoundingBox
	{
		public readonly Vector3 Min;
		public readonly Vector3 Max;
		public BoundingBox(Vector3 min, Vector3 max)
		{
			Min = min;
			Max = max;
		}
		public BoundingBox At(Vector3 position, Vector3 size)
		{
			return new BoundingBox(position, position + size);
		}
		public BoundingBox(float dx, float dy, float dz)
		{
			Min = new Vector3(-dx / 2, -dy / 2, -dz / 2);
			Max = new Vector3(dx / 2, dy / 2, dz / 2);
		}
		public Vector3 Size => Max - Min;
		public Vector3 Center => (Min + Max) / 2;
		public static BoundingBox operator +(BoundingBox a, Vector3 b)
		{
			return new BoundingBox(a.Min + b, a.Max + b);
		}
		public bool Intersects(BoundingBox b)
		{
			if (Max.x < b.Min.x) return false;
			if (Max.y < b.Min.y) return false;
			if (Max.z < b.Min.z) return false;
			if (Min.x > b.Max.x) return false;
			if (Min.y > b.Max.y) return false;
			if (Min.z > b.Max.z) return false;
			return true;
		}
		public override string ToString() => $"{Center}, s={Size}";
	}

	public class BoundingSphere
	{
		public Vector3 Center;
		public float Radius;
	}

	class OrthoNormalBasis
	{
		public readonly Vector3 U;
		public readonly Vector3 V;
		public readonly Plane Plane;
		public readonly Vector3 PlaneOrigin;
		public OrthoNormalBasis(Plane plane)
		{
			var rightvector = plane.Normal.RandomNonParallelVector();
			V = Vector3.Cross(plane.Normal,rightvector).normalized;
			U = Vector3.Cross(V,plane.Normal);
			Plane = plane;
			PlaneOrigin = plane.Normal * plane.W;
		}
		public Vector2 To2D(Vector3 vec3)
		{
			return new Vector2(Vector3.Dot(vec3,U), Vector3.Dot(vec3,V));
		}
		public Vector3 To3D(Vector2 vec2)
		{
			return PlaneOrigin + U * vec2.x + V * vec2.y;
		}
	}

	class Line2D
	{
		readonly Vector2 normal;
		//readonly float w;
		public Line2D(Vector2 normal, float w)
		{
			var l = normal.magnitude;
			w *= l;
			normal = normal * (1.0f / l);
			this.normal = normal;
			//this.w = w;
		}
		public Vector2 Direction => normal.Normal();
		public static Line2D FromPoints(Vector2 p1, Vector2 p2)
		{
			var direction = p2 - (p1);
            var normal = direction.Normal().normalized * -1f;
			var w = p1.Dot(normal);
			return new Line2D(normal, w);
		}
	}
}

