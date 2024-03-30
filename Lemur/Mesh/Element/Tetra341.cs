using System;

using Iguana.IguanaMesh.ITypes;

namespace Lemur.Mesh.Element
{
    public class Tetra341 : LeElementBase
    {
        public Tetra341(int[] nodeIds)
         : base(LeElementType.Tetra341, nodeIds)
        {
        }

        public Tetra341(int id, int[] nodeIds)
         : base(LeElementType.Tetra341, id, nodeIds)
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
                return -1;
            }

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
                case 0 when surfaceNodeIds[1] == 1 && surfaceNodeIds[2] == 3:
                    return 2;
                case 1 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 3:
                    return 3;
                case 0 when surfaceNodeIds[1] == 2 && surfaceNodeIds[2] == 3:
                    return 4;
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
                    return new int[] { NodeIds[0], NodeIds[1], NodeIds[3] };
                case 3:
                    return new int[] { NodeIds[1], NodeIds[2], NodeIds[3] };
                case 4:
                    return new int[] { NodeIds[2], NodeIds[0], NodeIds[3] };
                default:
                    throw new ArgumentException("Invalid surface id.");
            }
        }

        public Tetra342 ToQuadric(LeNodeList nodes)
        {
            throw new NotImplementedException();
        }

        public static Tetra341 FromIguanaElement(ITetrahedronElement element)
        {
            int id = element.Key;
            int[] nodeIds = new int[]
            {
                element.Vertices[3],
                element.Vertices[1],
                element.Vertices[0],
                element.Vertices[2]
            };
            return new Tetra341(id, nodeIds);
        }
    }
}
