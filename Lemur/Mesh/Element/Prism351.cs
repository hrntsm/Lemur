using System;
using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
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

        public Prism351(int[] nodeIds) : base(nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 6)
            {
                throw new ArgumentException("Prism351 requires 6 nodes.");
            }
        }
    }
}
