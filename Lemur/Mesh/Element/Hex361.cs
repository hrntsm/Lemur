using System;
using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public class Hex361 : LeSolidElementBase
    {
        public override LeElementType ElementType => LeElementType.Hex361;
        public override Dictionary<int, int[]> GetFace()
        {
            return new Dictionary<int, int[]>{
                {1, new int[] { NodeIds[0], NodeIds[1], NodeIds[2], NodeIds[3] }},
                {2, new int[] { NodeIds[4], NodeIds[5], NodeIds[6], NodeIds[7] }},
                {3, new int[] { NodeIds[0], NodeIds[1], NodeIds[5], NodeIds[4] }},
                {4, new int[] { NodeIds[1], NodeIds[2], NodeIds[6], NodeIds[5] }},
                {5, new int[] { NodeIds[2], NodeIds[3], NodeIds[7], NodeIds[6] }},
                {6, new int[] { NodeIds[3], NodeIds[0], NodeIds[4], NodeIds[7] }}
            };
        }

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

        public override int GetSurfaceId(int[] ids)
        {
            if (ids.Length != 4)
            {
                return -1;
            }

            int[] surfaceNodeIds = new int[4];
            for (int i = 0; i < 4; i++)
            {
                surfaceNodeIds[i] = Array.IndexOf(NodeIds, ids[i]);
            }

            if (surfaceNodeIds[0] == -1 || surfaceNodeIds[1] == -1 || surfaceNodeIds[2] == -1 || surfaceNodeIds[3] == -1)
            {
                return -1;
            }
            Array.Sort(surfaceNodeIds);

            switch (surfaceNodeIds[0])
            {
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 2 && surfaceNodeIds[3] == 3:
                    return 1;
                case 4 when surfaceNodeIds[1] == 5 && surfaceNodeIds[2] == 6 && surfaceNodeIds[3] == 7:
                    return 2;
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 4 && surfaceNodeIds[3] == 5:
                    return 3;
                case 1 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 5 && surfaceNodeIds[3] == 6:
                    return 4;
                case 2 when surfaceNodeIds[1] == 3 && surfaceNodeIds[2] == 6 && surfaceNodeIds[3] == 7:
                    return 5;
                case 0 when surfaceNodeIds[1] == 3 && surfaceNodeIds[2] == 4 && surfaceNodeIds[3] == 7:
                    return 6;
                default:
                    return -1;
            }
        }

        public override int[] GetSurfaceNodesFromId(int id)
        {
            return id >= 1 && id <= 6
             ? GetFace()[id]
             : throw new ArgumentException("Invalid surface id.");
        }
    }
}
