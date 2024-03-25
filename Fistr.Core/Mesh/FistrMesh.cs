using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Fistr.Core.Mesh.Element;

namespace Fistr.Core.Mesh
{
    public class FistrMesh
    {
        public FistrNodeList Nodes { get; }
        public FistrElementList[] Elements
        {
            get
            {
                return _elements.ToArray();
            }
        }

        private readonly string _header;
        private readonly List<FistrElementList> _elements;

        public FistrMesh(string header)
        {
            _header = header;
            Nodes = new FistrNodeList();
            _elements = new List<FistrElementList>();
        }

        public void AddNode(FistrNode node)
        {
            Nodes.Add(node);
        }

        public void AddNodes(IEnumerable<FistrNode> nodes)
        {
            Nodes.AddRange(nodes);
        }

        public void AddElement(FistrElementBase element)
        {
            CheckNodeExistence(element);
            FistrElementList list = _elements.Find(e => e.ElementType == element.ElementType);
            if (list == null)
            {
                list = new FistrElementList(element.ElementType);
                _elements.Add(list);
            }
            list.AddElement(element);
        }

        private void CheckNodeExistence(FistrElementBase element)
        {
            IEnumerable<int> nodeIds = Nodes.Select(n => n.Id);
            foreach (int nodeId in element.NodeIds)
            {
                if (!nodeIds.Contains(nodeId))
                {
                    throw new ArgumentException($"NodeID:{nodeId} does not exist in this mesh node list.");
                }
            }
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine("!HEADER");
            sb.AppendLine(" " + _header);
            sb.Append(Nodes.ToMsh());
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
