using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Lemur.Post.Mesh;


namespace Lemur.Mesh.Element
{
    public abstract class LeElementBase
    {
        public abstract LeElementType ElementType { get; }
        public int Id { get; private set; }
        public int[] NodeIds { get; private set; }
        public LeElementalResult[] ElementalResults => _elementalResults.ToArray();
        private readonly List<LeElementalResult> _elementalResults = new List<LeElementalResult>();

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

        public void ShiftIds(int elemShift, int nodeShift)
        {
            Id += elemShift;
            for (int i = 0; i < NodeIds.Length; i++)
            {
                NodeIds[i] += nodeShift;
            }
        }

        public void AddResult(LeElementalResult elementalResult)
        {
            _elementalResults.Add(elementalResult);
        }

        public void ClearResults()
        {
            _elementalResults.Clear();
        }
    }
}
