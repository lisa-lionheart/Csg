﻿using NUnit.Framework;
using System;
using static Csg.Solids;

namespace Csg.Test
{
	[TestFixture]
	public class CubeTest : SolidTest
	{
		[Test]
		public void Unit()
		{
			var s = Cube(1);
			Assert.AreEqual(6, s.Polygons.Count);
			AssertAcceptedStl(s, "CubeTest");
		}

		[Test]
		public void UnitNonCentered()
		{
			var s = Cube(1, center: false);
			Assert.AreEqual(6, s.Polygons.Count);
			AssertAcceptedStl(s, "CubeTest");
		}

		[Test]
		public void UnitCentered()
		{
			var s = Cube(1, center: true);
			Assert.AreEqual(6, s.Polygons.Count);
			var p0 = s.Polygons[0];
			Assert.GreaterOrEqual(p0.Plane.W, 0.4);
			Assert.LessOrEqual(p0.Plane.W, 0.6);
			AssertAcceptedStl(s, "CubeTest");
		}

		[Test]
		public void BigRadius()
		{
			var s = Cube(1.0e12f);
			AssertAcceptedStl(s, "CubeTest");
		}
	}
}

