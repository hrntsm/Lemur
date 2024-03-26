namespace Fistr.Core.Mesh.Element
{
    public class Prism352 : FistrElementBase
    {
        public Prism352(FistrElementType elementType, int[] nodeIds)
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
    }
}
