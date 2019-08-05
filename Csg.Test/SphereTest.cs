using NUnit.Framework;
using System;
using static Csg.Solids;

namespace Csg.Test
{
	[TestFixture]
	public class SphereTest : SolidTest
	{
		[Test]
		public void Unit()
		{
			var sphere = Sphere(1);
			Assert.AreEqual(72, sphere.Polygons.Count);
			var p0 = sphere.Polygons[0];
			Assert.GreaterOrEqual(p0.Plane.W, 0.9);
			Assert.LessOrEqual(p0.Plane.W, 1.1);
			AssertAcceptedStl(sphere, "SphereTest");
		}

		[Test]
		public void BigRadius()
		{
			var sphere = Sphere(1.0e8f);
			var p0 = sphere.Polygons[0];
			Assert.GreaterOrEqual(p0.Plane.W, 0.9e8);
			Assert.LessOrEqual(p0.Plane.W, 1.1e8);
			AssertAcceptedStl(sphere, "SphereTest");
		}
	}
}

