using System;
using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    [Serializable]
    public class Tetra342 : LeSolidElementBase
    {
        public override LeElementType ElementType => LeElementType.Tetra342;
        public override Dictionary<int, int[]> EdgeNodes => new Dictionary<int, int[]>
        {
            { 1, new int[] { NodeIds[0], NodeIds[1], NodeIds[6] } },
            { 2, new int[] { NodeIds[0], NodeIds[2], NodeIds[5] } },
            { 3, new int[] { NodeIds[0], NodeIds[3], NodeIds[7] } },
            { 4, new int[] { NodeIds[1], NodeIds[2], NodeIds[4] } },
            { 5, new int[] { NodeIds[1], NodeIds[3], NodeIds[8] } },
            { 6, new int[] { NodeIds[2], NodeIds[3], NodeIds[9] } }
        };
        public override Dictionary<int, int[]> FaceNodes => new Dictionary<int, int[]>{
            { 1, new int[] { NodeIds[0], NodeIds[6], NodeIds[1], NodeIds[4], NodeIds[2], NodeIds[5] } },
            { 2, new int[] { NodeIds[0], NodeIds[6], NodeIds[1], NodeIds[8], NodeIds[3], NodeIds[7] } },
            { 3, new int[] { NodeIds[1], NodeIds[4], NodeIds[2], NodeIds[9], NodeIds[3], NodeIds[8] } },
            { 4, new int[] { NodeIds[2], NodeIds[5], NodeIds[0], NodeIds[9], NodeIds[3], NodeIds[7] } }
        };
        public override int[][] NodeFacesArray => new int[][] {
            new int[] { 1, 7, 2, 5, 3, 6 },
            new int[] { 1, 7, 2, 9, 4, 8 },
            new int[] { 2, 5, 3, 10, 4, 9 },
            new int[] { 3, 6, 1, 10, 4, 8 }
        };

        public Tetra342(int[] nodeIds) : base(nodeIds)
        {
        }

        public Tetra342(int id, int[] nodeIds) : base(id, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 10)
            {
                throw new ArgumentException("Tetra342 requires 9 nodes.");
            }
        }

        public void SetNodeId(int index, int nodeId)
        {
            if (index < 0 || index > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 8.");
            }

            NodeIds[index] = nodeId;
        }

        public static Dictionary<int, Tetra342> CreateFromLinearElementList(LeElementList linearElementList)
        {
            if (linearElementList.ElementType != LeElementType.Tetra341)
            {
                throw new ArgumentException("Tetra342 can only be created from Tetra341.");
            }

            var tetra342Dict = new Dictionary<int, Tetra342>();
            foreach (LeElementBase elem in linearElementList)
            {
                int[] nodeIds = new int[]
                {
                    elem.NodeIds[0],
                    elem.NodeIds[1],
                    elem.NodeIds[2],
                    elem.NodeIds[3],
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1
                };
                var tetra342 = new Tetra342(elem.Id, nodeIds);
                tetra342Dict.Add(elem.Id, tetra342);
            }

            return tetra342Dict;
        }

        public static int GetEdgeNodeIndex(int id1, int id2)
        {
            if (id1 > id2)
            {
                (id2, id1) = (id1, id2);
            }

            if (id1 == 0 && id2 == 1)
            {
                return 6;
            }
            else if (id1 == 0 && id2 == 2)
            {
                return 5;
            }
            else if (id1 == 0 && id2 == 3)
            {
                return 7;
            }
            else if (id1 == 1 && id2 == 2)
            {
                return 4;
            }
            else if (id1 == 1 && id2 == 3)
            {
                return 8;
            }
            else if (id1 == 2 && id2 == 3)
            {
                return 9;
            }
            else
            {
                throw new ArgumentException("Invalid edge node.");
            }
        }
    }
}
