using System;
using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    [Serializable]
    public abstract class LeSolidElementBase : LeElementBase
    {
        public int FaceCount { get { return FaceNodes.Count; } }
        /// <summary>
        /// Dictionary of face ids to node ids.
        /// </summary>
        /// <value>key:faceId, value:nodeIds</value>
        public abstract Dictionary<int, int[]> FaceNodes { get; }
        public abstract int[][] NodeFacesArray { get; }

        public int EdgeCount { get { return EdgeNodes.Count; } }
        public abstract Dictionary<int, int[]> EdgeNodes { get; }

        protected LeSolidElementBase(int[] nodeIds)
         : base(nodeIds)
        {
        }

        protected LeSolidElementBase(int id, int[] nodeIds)
         : base(id, nodeIds)
        {
        }

        public int[] FaceToNodes(int faceId)
        {
            return FaceNodes.TryGetValue(faceId, out int[] nodeIds)
             ? nodeIds
             : Array.Empty<int>();
        }

        public int[] EdgeToNodes(int edgeId)
        {
            return EdgeNodes.TryGetValue(edgeId, out int[] nodeIds)
             ? nodeIds
             : Array.Empty<int>();
        }
    }
}
