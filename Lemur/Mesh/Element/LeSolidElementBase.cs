using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public abstract class LeSolidElementBase : LeElementBase
    {
        public abstract Dictionary<int, int[]> GetFace();
        public int FaceCount { get { return GetFace().Count; } }

        protected LeSolidElementBase(LeElementType elementType, int[] nodeIds)
         : base(elementType, nodeIds)
        {
        }

        protected LeSolidElementBase(LeElementType elementType, int id, int[] nodeIds)
         : base(elementType, id, nodeIds)
        {
        }
    }
}
