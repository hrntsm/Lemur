using System.Globalization;
using System.Text;


namespace Fistr.Core.Mesh.Element
{
    public abstract class FistrElementBase
    {
        public FistrElementType ElementType { get; private set; }
        public int Id { get; private set; }
        public int[] NodeIds { get; private set; }

        public FistrElementBase(FistrElementType elementType, int[] nodeIds)
        {
            Ctor(elementType, -1, nodeIds);
        }

        public FistrElementBase(FistrElementType elementType, int id, int[] nodeIds)
        {
            Ctor(elementType, id, nodeIds);
        }

        private void Ctor(FistrElementType elementType, int id, int[] nodeIds)
        {
            ElementType = elementType;
            Id = id;
            CheckNodeLength(nodeIds.Length);
            NodeIds = nodeIds;
        }

        protected abstract void CheckNodeLength(int length);
        public abstract int GetSurfaceId(int[] ids);

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
