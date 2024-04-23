using System;
using System.Collections.Generic;

using Iguana.IguanaMesh.ITypes;

namespace Lemur.Mesh.Element
{
    [Serializable]
    public class Hexa361 : LeSolidElementBase
    {
        public override LeElementType ElementType => LeElementType.Hex361;
        public override Dictionary<int, int[]> FaceNodes => new Dictionary<int, int[]>{
            { 1, new int[] { NodeIds[0], NodeIds[1], NodeIds[2], NodeIds[3] } },
            { 2, new int[] { NodeIds[4], NodeIds[5], NodeIds[6], NodeIds[7] } },
            { 3, new int[] { NodeIds[0], NodeIds[1], NodeIds[5], NodeIds[4] } },
            { 4, new int[] { NodeIds[1], NodeIds[2], NodeIds[6], NodeIds[5] } },
            { 5, new int[] { NodeIds[2], NodeIds[3], NodeIds[7], NodeIds[6] } },
            { 6, new int[] { NodeIds[3], NodeIds[0], NodeIds[4], NodeIds[7] } }
        };

        public override int[][] NodeFacesArray => new int[][]
        {
            new int[] { 1, 3, 6 },
            new int[] { 1, 3, 4 },
            new int[] { 1, 4, 5 },
            new int[] { 2, 3, 6 },
            new int[] { 2, 3, 4 },
            new int[] { 2, 4, 5 },
            new int[] { 2, 5, 6 },
        };

        public override Dictionary<int, int[]> EdgeNodes => throw new NotImplementedException();

        public Hexa361(int[] nodeIds) : base(nodeIds)
        {
        }

        public Hexa361(int id, int[] nodeIds) : base(id, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 8)
            {
                throw new ArgumentException("Hex361 requires 8 nodes.");
            }
        }

        public static Hexa361 FromIguanaElement(IHexahedronElement element, int idOffset)
        {
            int id = element.Key + idOffset;
            int[] nodeIds = new int[]
            {
                element.Vertices[4],
                element.Vertices[5],
                element.Vertices[1],
                element.Vertices[0],
                element.Vertices[7],
                element.Vertices[6],
                element.Vertices[2],
                element.Vertices[3]
            };
            return new Hexa361(id, nodeIds);
        }
    }
}
