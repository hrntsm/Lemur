using System;

namespace Fistr.Core.Mesh.Element
{
    public class Prism351 : FistrElementBase
    {
        public Prism351(int[] nodeIds)
         : base(FistrElementType.Prism351, nodeIds)
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
                    throw new ArgumentException("Prism351 surface requires 3 or 4 nodes.");
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
                throw new ArgumentException("Surface nodes not found.");
            }
            Array.Sort(surfaceNodeIds);

            switch (surfaceNodeIds[0])
            {
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 2:
                    return 1;
                case 3 when surfaceNodeIds[1] == 4 && surfaceNodeIds[2] == 5:
                    return 2;
                default:
                    throw new ArgumentException("Surface node not found.");
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
                throw new ArgumentException("Surface nodes not found.");
            }
            Array.Sort(surfaceNodeIds);

            switch (surfaceNodeIds[0])
            {
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 3 && surfaceNodeIds[3] == 4:
                    return 2;
                case 1 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 4 && surfaceNodeIds[3] == 5:
                    return 3;
                case 0 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 3 && surfaceNodeIds[3] == 5:
                    return 4;
                default:
                    throw new ArgumentException("Surface node not found.");
            }
        }
    }
}
