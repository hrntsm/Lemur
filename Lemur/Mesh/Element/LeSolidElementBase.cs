using System;
using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public abstract class LeSolidElementBase : LeElementBase
    {
        public int FaceCount { get { return FaceNodes.Count; } }
        public abstract Dictionary<int, int[]> FaceNodes { get; }
        public abstract int[][] NodeFacesArray { get; }
        public Dictionary<int, int[]> NodeFaces
        {
            get
            {
                if (NodeIds.Length != NodeFacesArray.Length)
                {
                    throw new ArgumentException("NodeFaceArray length must match NodeIds length.");
                }
                var nodeFaces = new Dictionary<int, int[]>();
                for (int i = 0; i < NodeIds.Length; i++)
                {
                    nodeFaces.Add(NodeIds[i], NodeFacesArray[i]);
                }
                return nodeFaces;
            }
        }

        protected LeSolidElementBase(int[] nodeIds)
         : base(nodeIds)
        {
        }

        protected LeSolidElementBase(int id, int[] nodeIds)
         : base(id, nodeIds)
        {
        }

        public int[] NodeToFaces(int nodeId)
        {
            return NodeFaces.TryGetValue(nodeId, out int[] faceIds)
             ? faceIds
             : Array.Empty<int>();
        }

        public int[] FaceToNodes(int faceId)
        {
            return FaceNodes.TryGetValue(faceId, out int[] nodeIds)
             ? nodeIds
             : Array.Empty<int>();
        }
    }
}
