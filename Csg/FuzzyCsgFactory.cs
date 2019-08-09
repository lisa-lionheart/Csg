using System.Collections.Generic;
#nullable enable
namespace Csg
{
    class FuzzyCsgFactory
	{
		readonly VertexFactory vertexfactory = new VertexFactory (1.0e-5f);
		readonly PlaneFactory planefactory = new PlaneFactory(1.0e-5f);
		readonly Dictionary<string, PolygonShared> polygonsharedfactory = new Dictionary<string, PolygonShared>();

		public PolygonShared GetPolygonShared(PolygonShared sourceshared)
		{
			var hash = sourceshared.Hash;
			PolygonShared result;
			if (polygonsharedfactory.TryGetValue(hash, out result))
			{
				return result;
			}
			else
			{
				polygonsharedfactory.Add(hash, sourceshared);
				return sourceshared;
			}
		}

		public Vertex GetVertex(Vertex sourcevertex)
		{
			var result = vertexfactory.LookupOrCreate(ref sourcevertex);
			return result;
		}

		public Plane GetPlane(Plane sourceplane)
		{
			var result = planefactory.LookupOrCreate(sourceplane);
			return result;
		}

		public Polygon GetPolygon(Polygon sourcepolygon)
		{
			var newplane = GetPlane(sourcepolygon.Plane);
			var newshared = GetPolygonShared(sourcepolygon.Shared);
			var newvertices = new List<Vertex>(sourcepolygon.Vertices);
			for (int i = 0; i < newvertices.Count; i++)
			{
				newvertices[i] = GetVertex(newvertices[i]);
			}
			// two vertices that were originally very close may now have become
			// truly identical (referring to the same CSG.Vertex object).
			// Remove duplicate vertices:
			var newvertices_dedup = new List<Vertex>();
			if (newvertices.Count > 0)
			{
				var prevvertextag = newvertices[newvertices.Count - 1].Tag;
				foreach (var vertex in newvertices) {
					var vertextag = vertex.Tag;
					if (vertextag != prevvertextag)
					{
						newvertices_dedup.Add(vertex);
					}
					prevvertextag = vertextag;
				}
			}
			// If it's degenerate, remove all vertices:
			if (newvertices_dedup.Count < 3)
			{
				newvertices_dedup = new List<Vertex>();
			}
			return new Polygon(newvertices_dedup, newshared, newplane);
		}

		
        public List<Polygon> GetCsg(IEnumerable<Polygon> polygons) {
            var newpolygons = new List<Polygon>();
            foreach (var polygon in polygons) {
                var newpolygon = GetPolygon(polygon);
                if (newpolygon.Vertices.Count >= 3) {
                    newpolygons.Add(newpolygon);
                }
            }
            return newpolygons;
        }
    }
}
