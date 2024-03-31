using System.Globalization;
using System.Text;


namespace Lemur.Mesh.Element
{
    public abstract class LeElementBase
    {
        public abstract LeElementType ElementType { get; }
        public int Id { get; private set; }
        public int[] NodeIds { get; private set; }

        public LeElementBase(int[] nodeIds)
        {
            Ctor(-1, nodeIds);
        }

        public LeElementBase(int id, int[] nodeIds)
        {
            Ctor(id, nodeIds);
        }

        private void Ctor(int id, int[] nodeIds)
        {
            Id = id;
            CheckNodeLength(nodeIds.Length);
            NodeIds = nodeIds;
        }

        protected abstract void CheckNodeLength(int length);
        public abstract int GetSurfaceId(int[] ids);
        public abstract int[] GetSurfaceNodesFromId(int id);

        public string ToMsh(int id)
        {
            var sb = new StringBuilder();
            sb.Append(id.ToString(CultureInfo.InvariantCulture).PadLeft(9, ' '));
            foreach (int nodeId in NodeIds)
            {
                sb.Append(',');
                sb.Append(nodeId.ToString(CultureInfo.InvariantCulture).PadLeft(9, ' '));
            }
            return sb.ToString();
        }

    }
}
