using System.Collections.Generic;
using System.IO;
using System.Text;

using Fistr.Core.Mesh.Element;

namespace Fistr.Core.Mesh
{
    public class FistrMesh
    {
        private readonly string _header;
        private readonly FistrNodeList _nodes;
        private readonly List<FistrElementList> _elements;

        public FistrMesh(string header)
        {
            _header = header;
            _nodes = new FistrNodeList();
            _elements = new List<FistrElementList>();
        }

        public void AddNode(FistrNode node)
        {
            _nodes.AddNode(node);
        }

        public void AddElement(FistrElementBase element)
        {
            FistrElementList list = _elements.Find(e => e.ElementType == element.ElementType);
            if (list == null)
            {
                list = new FistrElementList(element.ElementType);
                _elements.Add(list);
            }
            list.AddElement(element);
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine("!HEADER");
            sb.AppendLine(" " + _header);
            sb.Append(_nodes.ToMsh());
            int startId = 1;
            foreach (FistrElementList elementList in _elements)
            {
                sb.Append(elementList.ToMsh(startId));
                startId += elementList.Count;
            }

            return sb.ToString();
        }

        public void Serialize(string path)
        {
            File.WriteAllText(path, ToMsh());
        }
    }
}
