using System.Collections.Generic;
#nullable enable
namespace Csg
{
    class PlaneFactory
	{
		static readonly KeyComparer keyComparer = new KeyComparer ();
		readonly Dictionary<Key, Plane> lookuptable = new Dictionary<Key, Plane> (keyComparer);
		readonly float multiplier;
		public PlaneFactory (float tolerance)
		{
			multiplier = 1.0f / tolerance;
		}
		public Plane LookupOrCreate (Plane plane)
		{
			var key = new Key {
				X = (int)(plane.Normal.x * multiplier + 0.5),
				Y = (int)(plane.Normal.y * multiplier + 0.5),
				Z = (int)(plane.Normal.z * multiplier + 0.5),
				W = (int)(plane.W * multiplier + 0.5),
			};
			if (lookuptable.TryGetValue (key, out var p))
				return p;
			lookuptable.Add (key, plane);
			return plane;
		}
		struct Key
		{
			public int X, Y, Z, W;
		}
		class KeyComparer : IEqualityComparer<Key>
		{
			public bool Equals (Key x, Key y)
			{
				return x.X == y.X && x.Y == y.Y && x.Z == y.Z && x.W == y.W;
			}

			public int GetHashCode (Key k)
			{
				var hashCode = 1570706993;
				hashCode = hashCode * -1521134295 + k.X.GetHashCode ();
				hashCode = hashCode * -1521134295 + k.Y.GetHashCode ();
				hashCode = hashCode * -1521134295 + k.Z.GetHashCode ();
				hashCode = hashCode * -1521134295 + k.W.GetHashCode ();
				return hashCode;
			}
		}
	}
}
