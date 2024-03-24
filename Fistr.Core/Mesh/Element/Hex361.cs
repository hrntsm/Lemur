using System;

namespace Fistr.Core.Mesh.Element
{
    public class Hex361 : FistrElementBase
    {
        public Hex361(int[] nodeIds)
         : base(FistrElementType.Hex361, nodeIds)
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
                throw new ArgumentException("Hex361 surface requires 4 nodes.");
            }

            int[] surfaceNodeIds = new int[4];
            for (int i = 0; i < 4; i++)
            {
                surfaceNodeIds[i] = Array.IndexOf(NodeIds, ids[i]);
            }

            if (surfaceNodeIds[0] == -1 || surfaceNodeIds[1] == -1 || surfaceNodeIds[2] == -1 || surfaceNodeIds[3] == -1)
            {
                throw new ArgumentException("Surface nodes not found.");
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
                    throw new ArgumentException("Surface node not found.");
            }
        }
    }
}
