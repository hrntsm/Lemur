using System;
using System.Collections.Generic;

using Iguana.IguanaMesh.ITypes;

namespace Lemur.Mesh.Element
{
    [Serializable]
    public class Prism351 : LeSolidElementBase
    {
        public override LeElementType ElementType => LeElementType.Prism351;
        public override Dictionary<int, int[]> FaceNodes => new Dictionary<int, int[]> {
            { 1, new int[] { NodeIds[0], NodeIds[1], NodeIds[2] }},
            { 2, new int[] { NodeIds[3], NodeIds[4], NodeIds[5] }},
            { 3, new int[] { NodeIds[0], NodeIds[1], NodeIds[4], NodeIds[3] }},
            { 4, new int[] { NodeIds[1], NodeIds[2], NodeIds[5], NodeIds[4] }},
            { 5, new int[] { NodeIds[2], NodeIds[0], NodeIds[3], NodeIds[5] }}
        };

        public override int[][] NodeFacesArray => new int[][]{
            new int[] { 1, 3, 5 },
            new int[] { 1, 3, 4 },
            new int[] { 1, 4, 5 },
            new int[] { 2, 3, 5 },
            new int[] { 2, 4, 5 }
        };

        public override Dictionary<int, int[]> EdgeNodes => throw new NotImplementedException();

        public Prism351(int[] nodeIds) : base(nodeIds)
        {
        }

        public Prism351(int id, int[] nodeIds) : base(id, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 6)
            {
                throw new ArgumentException("Prism351 requires 6 nodes.");
            }
        }

        public static Prism351 FromIguanaElement(IPrismElement element, int idOffset)
        {
            int id = element.Key + idOffset;
            int[] nodeIds = new int[]
            {
                element.Vertices[2],
                element.Vertices[0],
                element.Vertices[1],
                element.Vertices[5],
                element.Vertices[3],
                element.Vertices[4]
            };
            return new Prism351(id, nodeIds);
        }
    }
}
