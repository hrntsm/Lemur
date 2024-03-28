using System;

namespace Lemur.Mesh.Element
{
    public class Tetra342 : LeElementBase
    {
        public Tetra342(int[] nodeIds)
         : base(LeElementType.Tetra342, nodeIds)
        {
        }

        protected override void CheckNodeLength(int length)
        {
            throw new NotImplementedException();
        }

        public override int GetSurfaceId(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}
