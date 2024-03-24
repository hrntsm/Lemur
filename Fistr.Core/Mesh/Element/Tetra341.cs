using System;

namespace Fistr.Core.Mesh.Element
{
    public class Tetra341 : FistrElementBase
    {
        public Tetra341(int[] nodeIds)
         : base(FistrElementType.Tetra341, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            if (length != 4)
            {
                throw new ArgumentException("Tetra341 requires 4 nodes.");
            }
        }

        public override int GetSurfaceId(int[] ids)
        {
            if (ids.Length != 3)
            {
                throw new ArgumentException("Tetra341 surface requires 3 nodes.");
            }

            int[] surfaceNodeIds = new int[3];
            for (int i = 0; i < 3; i++)
            {
                surfaceNodeIds[i] = Array.IndexOf(NodeIds, ids[i]);
            }

            if (surfaceNodeIds[0] == -1 || surfaceNodeIds[1] == -1 || surfaceNodeIds[2] == -1)
            {
                throw new ArgumentException("Surface nodes not found.");
            }
            Array.Sort(surfaceNodeIds);

            switch (surfaceNodeIds[0])
            {
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 2:
                    return 1;
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 3:
                    return 2;
                case 1 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 3:
                    return 3;
                case 0 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 3:
                    return 4;
                default:
                    throw new ArgumentException("Surface node not found.");
            }
        }
    }
}
