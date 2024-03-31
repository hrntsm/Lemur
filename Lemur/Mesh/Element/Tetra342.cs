using System;
using System.Collections.Generic;

namespace Lemur.Mesh.Element
{
    public class Tetra342 : LeSolidElementBase
    {
        public override Dictionary<int, int[]> GetFace()
        {
            throw new NotImplementedException();
        }

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

        public override int[] GetSurfaceNodesFromId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
