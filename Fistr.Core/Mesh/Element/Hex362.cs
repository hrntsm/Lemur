namespace Fistr.Core.Mesh.Element
{
    public class Hex362 : FistrElementBase
    {
        public Hex362(FistrElementType elementType, int[] nodeIds)
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
