using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public class Prism352 : LeSolidElementBase
    {
        public override Dictionary<int, int[]> GetFace()
        {
            throw new System.NotImplementedException();
        }

        public Prism352(LeElementType elementType, int[] nodeIds)
         : base(elementType, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            throw new System.NotImplementedException();
        }

        public override int GetSurfaceId(int[] ids)
        {
            throw new System.NotImplementedException();
        }

        public override int[] GetSurfaceNodesFromId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
