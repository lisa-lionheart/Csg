using System;
using UnityEngine;

namespace Csg
{/*
	public struct Vector3 : IEquatable<Vector3>
	{
		public float x, y, z;

		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public bool Equals(Vector3 a)
		{
#pragma warning disable RECS0018 // Comparison of floating point numbers with equality operator
			return x == a.x && y == a.y && z == a.z;
#pragma warning restore RECS0018 // Comparison of floating point numbers with equality operator
		}

		public float Length
		{
			get { return Mathf.Sqrt(x * x + y * y + z * z); }
		}


		public float Dot(Vector3 a)
		{
			return x * a.x + y * a.y + z * a.z;
		}

		public Vector3 Cross(Vector3 a)
		{
			return new Vector3(
				y * a.z - z * a.y,
				z * a.x - x * a.z,
				x * a.y - y * a.x);
		}

		public Vector3 Unit
		{
			get
			{
				var d = Length;
				return new Vector3(x / d, y / d, z / d);
			}
		}

		public Vector3 Negated
		{
			get
			{
				return new Vector3(-x, -y, -z);
			}
		}



		public Vector3 Min(Vector3 other)
		{
			return new Vector3(Mathf.Min(x, other.x), Mathf.Min(y, other.y), Mathf.Min(z, other.z));
		}

		public Vector3 Max(Vector3 other)
		{
			return new Vector3(Mathf.Max(x, other.x), Mathf.Max(y, other.y), Mathf.Max(z, other.z));
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Vector3 operator *(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}
		public static Vector3 operator *(Vector3 a, float b)
		{
			return new Vector3(a.x * b, a.y * b, a.z * b);
		}
		public static Vector3 operator /(Vector3 a, float b)
		{
			return new Vector3(a.x / b, a.y / b, a.z / b);
		}
		public static Vector3 operator *(Vector3 a, Matrix4x4 b)
		{
			return b.LeftMultiply1x3Vector(a);
		}

		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "[{0:0.000}, {1:0.000}, {2:0.000}]", x, y, z);
		}

	}*/

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
	/*
    public struct Vector2 : IEquatable<Vector2>
	{
		public float x, y;

		public Vector2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public bool Equals(Vector2 a)
		{
#pragma warning disable RECS0018 // Comparison of floating point numbers with equality operator
			return x == a.x && y == a.y;
#pragma warning restore RECS0018 // Comparison of floating point numbers with equality operator
		}

		public float magnitude
		{
			get { return Mathf.Sqrt(x * x + y * y); }
		}


		public Vector2 normalized
		{
			get
			{
				var d = magnitude;
				return new Vector2(x / d, y / d);
			}
		}

		public Vector2 Negated => new Vector2(-x, -y);


		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}
		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}
		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x, a.y * b.y);
		}
		public static Vector2 operator *(Vector2 a, float b)
		{
			return new Vector2(a.x * b, a.y * b);
		}
		public static Vector2 operator /(Vector2 a, float b)
		{
			return new Vector2(a.x / b, a.y / b);
		}

		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "[{0:0.000}, {1:0.000}]", x, y);
		}
	}
	*/
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

	public class Matrix4x4
	{
		readonly float[] elements;

		public bool IsMirroring = false;

		public Matrix4x4(float[] els)
		{
			elements = els;
		}

		public Matrix4x4()
			: this(new float[] {
				1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1
			})
		{
		}

		public float[] Elements => elements;

		public static Matrix4x4 Scaling(Vector3 vec)
		{
			var els = new[] {
				vec.x, 0, 0, 0, 0, vec.y, 0, 0, 0, 0, vec.z, 0, 0, 0, 0, 1
			};
			return new Matrix4x4(els);
		}

		public static Matrix4x4 Translation(Vector3 vec)
		{
			var els = new[] {
				1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, vec.x, vec.y, vec.z, 1
			};
			return new Matrix4x4(els);
		}

		public static Matrix4x4 RotationX(float degrees)
		{
			var radians = degrees * Mathf.PI * (1.0f / 180.0f);
			var cos = Mathf.Cos(radians);
			var sin = Mathf.Sin(radians);
			var els = new float[] {
				1, 0, 0, 0, 0, cos, sin, 0, 0, -sin, cos, 0, 0, 0, 0, 1
			};
			return new Matrix4x4(els);
		}

		public static Matrix4x4 RotationY(float degrees)
		{
			var radians = degrees * Mathf.PI * (1.0f / 180.0f);
			var cos = Mathf.Cos(radians);
			var sin = Mathf.Sin(radians);
			var els = new float[] {
				cos, 0, -sin, 0, 0, 1, 0, 0, sin, 0, cos, 0, 0, 0, 0, 1
			};
			return new Matrix4x4(els);
		}

		public static Matrix4x4 RotationZ(float degrees)
		{
			var radians = degrees * Mathf.PI * (1.0f / 180.0f);
			var cos = Mathf.Cos(radians);
			var sin = Mathf.Sin(radians);
			var els = new float[] {
				cos, sin, 0, 0, -sin, cos, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1
			};
			return new Matrix4x4(els);
		}

		public Vector3 LeftMultiply1x3Vector(Vector3 v)
		{
			var v0 = v.x;
			var v1 = v.y;
			var v2 = v.z;
			var v3 = 1;
			var x = v0 * this.elements[0] + v1 * this.elements[4] + v2 * this.elements[8] + v3 * this.elements[12];
			var y = v0 * this.elements[1] + v1 * this.elements[5] + v2 * this.elements[9] + v3 * this.elements[13];
			var z = v0 * this.elements[2] + v1 * this.elements[6] + v2 * this.elements[10] + v3 * this.elements[14];
			var w = v0 * this.elements[3] + v1 * this.elements[7] + v2 * this.elements[11] + v3 * this.elements[15];
			// scale such that fourth element becomes 1:
			if (w != 1)
			{
				var invw = 1.0f / w;
				x *= invw;
				y *= invw;
				z *= invw;
			}
			return new Vector3(x, y, z);
		}

		public static Matrix4x4 operator * (Matrix4x4 l, Matrix4x4 m)
		{
			// cache elements in local variables, for speedup:
			var this0  = l.elements[0];
			var this1  = l.elements[1];
			var this2  = l.elements[2];
			var this3  = l.elements[3];
			var this4  = l.elements[4];
			var this5  = l.elements[5];
			var this6  = l.elements[6];
			var this7  = l.elements[7];
			var this8  = l.elements[8];
			var this9  = l.elements[9];
			var this10 = l.elements[10];
			var this11 = l.elements[11];
			var this12 = l.elements[12];
			var this13 = l.elements[13];
			var this14 = l.elements[14];
			var this15 = l.elements[15];
			var m0 = m.elements[0];
			var m1 = m.elements[1];
			var m2 = m.elements[2];
			var m3 = m.elements[3];
			var m4 = m.elements[4];
			var m5 = m.elements[5];
			var m6 = m.elements[6];
			var m7 = m.elements[7];
			var m8 = m.elements[8];
			var m9 = m.elements[9];
			var m10 = m.elements[10];
			var m11 = m.elements[11];
			var m12 = m.elements[12];
			var m13 = m.elements[13];
			var m14 = m.elements[14];
			var m15 = m.elements[15];

			var result = new float[16];
			result[0] = this0 * m0 + this1 * m4 + this2 * m8 + this3 * m12;
			result[1] = this0 * m1 + this1 * m5 + this2 * m9 + this3 * m13;
			result[2] = this0 * m2 + this1 * m6 + this2 * m10 + this3 * m14;
			result[3] = this0 * m3 + this1 * m7 + this2 * m11 + this3 * m15;
			result[4] = this4 * m0 + this5 * m4 + this6 * m8 + this7 * m12;
			result[5] = this4 * m1 + this5 * m5 + this6 * m9 + this7 * m13;
			result[6] = this4 * m2 + this5 * m6 + this6 * m10 + this7 * m14;
			result[7] = this4 * m3 + this5 * m7 + this6 * m11 + this7 * m15;
			result[8] = this8 * m0 + this9 * m4 + this10 * m8 + this11 * m12;
			result[9] = this8 * m1 + this9 * m5 + this10 * m9 + this11 * m13;
			result[10] = this8 * m2 + this9 * m6 + this10 * m10 + this11 * m14;
			result[11] = this8 * m3 + this9 * m7 + this10 * m11 + this11 * m15;
			result[12] = this12 * m0 + this13 * m4 + this14 * m8 + this15 * m12;
			result[13] = this12 * m1 + this13 * m5 + this14 * m9 + this15 * m13;
			result[14] = this12 * m2 + this13 * m6 + this14 * m10 + this15 * m14;
			result[15] = this12 * m3 + this13 * m7 + this14 * m11 + this15 * m15;
			return new Matrix4x4(result);
		}
	}
}

