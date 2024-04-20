using System;
using System.Collections.Generic;

using Iguana.IguanaMesh.ITypes;

namespace Lemur.Mesh.Element
{
    public class Tetra341 : LeSolidElementBase
    {
        public override LeElementType ElementType => LeElementType.Tetra341;
        public override Dictionary<int, int[]> FaceNodes => new Dictionary<int, int[]>{
            { 1, new int[] { NodeIds[0], NodeIds[1], NodeIds[2] } },
            { 2, new int[] { NodeIds[0], NodeIds[1], NodeIds[3] } },
            { 3, new int[] { NodeIds[1], NodeIds[2], NodeIds[3] } },
            { 4, new int[] { NodeIds[2], NodeIds[0], NodeIds[3] } }
        };

        public override int[][] NodeFacesArray => new int[][] {
            new int[] { 1, 2, 4 },
            new int[] { 1, 2, 3 },
            new int[] { 1, 3, 4 },
            new int[] { 2, 3, 4 }
        };

        public Tetra341(int[] nodeIds) : base(nodeIds)
        {
        }

        public Tetra341(int id, int[] nodeIds) : base(id, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 4)
            {
                throw new ArgumentException("Tetra341 requires 4 nodes.");
            }
        }

        public static Tetra341 FromIguanaElement(ITetrahedronElement element, int idOffset)
        {
            int id = element.Key + idOffset;
            int[] nodeIds = new int[]
            {
                element.Vertices[3],
                element.Vertices[1],
                element.Vertices[0],
                element.Vertices[2]
            };
            return new Tetra341(id, nodeIds);
        }

        public static Tetra341[] FromIguanaElement(IPyramidElement element, int idOffset)
        {
            var tetras = new Tetra341[2];

            int id1 = element.Key + idOffset;
            int[] nodeIds1 = new int[]
            {
                element.Vertices[1],
                element.Vertices[3],
                element.Vertices[0],
                element.Vertices[4]
            };
            tetras[0] = new Tetra341(id1, nodeIds1);

            int id2 = element.Key + 1 + idOffset;
            int[] nodeIds2 = new int[]
            {
                element.Vertices[1],
                element.Vertices[2],
                element.Vertices[3],
                element.Vertices[4]
            };
            tetras[1] = new Tetra341(id2, nodeIds2);

            return tetras;
        }
    }
}
