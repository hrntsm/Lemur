using System;

namespace Fistr.Core.Mesh.Element
{
    public class Tetra342 : FistrElementBase
    {
        public Tetra342(int[] nodeIds)
         : base(FistrElementType.Tetra342, nodeIds)
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
