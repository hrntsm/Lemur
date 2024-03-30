using System;

namespace Lemur.Mesh.Element
{
    public class Prism351 : LeElementBase
    {
        public override int FaceCount => 5;

        public Prism351(int[] nodeIds)
         : base(LeElementType.Prism351, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 6)
            {
                throw new ArgumentException("Prism351 requires 6 nodes.");
            }
        }

        public override int GetSurfaceId(int[] ids)
        {
            switch (ids.Length)
            {
                case 3:
                    return GetSurfaceIdTri(ids);
                case 4:
                    return GetSurfaceIdQuad(ids);
                default:
                    return -1;
            }
        }

        private int GetSurfaceIdTri(int[] ids)
        {
            int[] surfaceNodeIds = new int[3];
            for (int i = 0; i < 3; i++)
            {
                surfaceNodeIds[i] = Array.IndexOf(NodeIds, ids[i]);
            }

            if (surfaceNodeIds[0] == -1 || surfaceNodeIds[1] == -1 || surfaceNodeIds[2] == -1)
            {
                return -1;
            }
            Array.Sort(surfaceNodeIds);

            switch (surfaceNodeIds[0])
            {
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 2:
                    return 1;
                case 3 when surfaceNodeIds[1] == 4 && surfaceNodeIds[2] == 5:
                    return 2;
                default:
                    return -1;
            }
        }

        private int GetSurfaceIdQuad(int[] ids)
        {
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
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 3 && surfaceNodeIds[3] == 4:
                    return 3;
                case 1 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 4 && surfaceNodeIds[3] == 5:
                    return 4;
                case 0 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 3 && surfaceNodeIds[3] == 5:
                    return 5;
                default:
                    return -1;
            }
        }

        public override int[] GetSurfaceNodesFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return new int[] { NodeIds[0], NodeIds[1], NodeIds[2] };
                case 2:
                    return new int[] { NodeIds[3], NodeIds[4], NodeIds[5] };
                case 3:
                    return new int[] { NodeIds[0], NodeIds[1], NodeIds[4], NodeIds[3] };
                case 4:
                    return new int[] { NodeIds[1], NodeIds[2], NodeIds[5], NodeIds[4] };
                case 5:
                    return new int[] { NodeIds[2], NodeIds[0], NodeIds[3], NodeIds[5] };
                default:
                    throw new ArgumentException("Invalid surface id.");
            }
        }
    }
}
