using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public class Hex362 : LeSolidElementBase
    {
        public override LeElementType ElementType => LeElementType.Hex362;
        public override Dictionary<int, int[]> GetFace()
        {
            throw new System.NotImplementedException();
        }

        public Hex362(int[] nodeIds) : base(nodeIds)
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
