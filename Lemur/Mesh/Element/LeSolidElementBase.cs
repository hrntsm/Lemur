using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public abstract class LeSolidElementBase : LeElementBase
    {
        public abstract Dictionary<int, int[]> GetFace();
        public int FaceCount { get { return GetFace().Count; } }

        protected LeSolidElementBase(int[] nodeIds)
         : base(nodeIds)
        {
        }

        protected LeSolidElementBase(int id, int[] nodeIds)
         : base(id, nodeIds)
        {
        }
    }
}
