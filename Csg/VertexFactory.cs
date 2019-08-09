using System.Collections.Generic;
#nullable enable
namespace Csg
{
    class VertexFactory
	{
		static readonly KeyComparer keyComparer = new KeyComparer ();
		readonly Dictionary<Key, Vertex> lookuptable = new Dictionary<Key, Vertex> (keyComparer);
		readonly float multiplier;
		public VertexFactory (float tolerance)
		{
			multiplier = 1.0f / tolerance;
		}
		public Vertex LookupOrCreate (ref Vertex vertex)
		{
			var key = new Key {
				X = (int)(vertex.Pos.x * multiplier + 0.5),
				Y = (int)(vertex.Pos.y * multiplier + 0.5),
				Z = (int)(vertex.Pos.z * multiplier + 0.5),
				U = (int)(vertex.Tex.x * multiplier + 0.5),
				V = (int)(vertex.Tex.y * multiplier + 0.5),
			};
			if (lookuptable.TryGetValue (key, out var v))
				return v;
			lookuptable.Add (key, vertex);
			return vertex;
		}
		struct Key
		{
			public int X, Y, Z, U, V;
		}
		class KeyComparer : IEqualityComparer<Key>
		{
			public bool Equals (Key x, Key y)
			{
				return x.X == y.X && x.Y == y.Y && x.Z == y.Z && x.U == y.U && x.V == y.V;
			}

			public int GetHashCode (Key k)
			{
				var hashCode = 1570706993;
				hashCode = hashCode * -1521134295 + k.X.GetHashCode ();
				hashCode = hashCode * -1521134295 + k.Y.GetHashCode ();
				hashCode = hashCode * -1521134295 + k.Z.GetHashCode ();
				hashCode = hashCode * -1521134295 + k.U.GetHashCode ();
				hashCode = hashCode * -1521134295 + k.V.GetHashCode ();
				return hashCode;
			}
		}
	}
}
