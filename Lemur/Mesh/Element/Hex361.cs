using System;
using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public class Hex361 : LeSolidElementBase
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

        public Hex361(int[] nodeIds) : base(nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 8)
            {
                throw new ArgumentException("Hex361 requires 8 nodes.");
            }
        }
    }
}
