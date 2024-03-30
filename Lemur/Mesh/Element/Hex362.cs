namespace Lemur.Mesh.Element
{
    public class Hex362 : LeElementBase
    {
        public override int FaceCount => 6;

        public Hex362(LeElementType elementType, int[] nodeIds)
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
