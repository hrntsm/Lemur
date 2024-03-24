using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fistr.Core.Mesh
{
    public class FistrNodeList : IFistrMesh
    {
        public List<FistrNode> Nodes { get; }

        public FistrNodeList()
        {
            Nodes = new List<FistrNode>();
        }

        public void AddNode(double x, double y, double z)
        {
            int id = Nodes.Max(n => n.Id) + 1;
            AddNode(id, x, y, z);
        }

        public void AddNode(int id, double x, double y, double z)
        {
            var node = new FistrNode(id, x, y, z);
            AddNode(node);
        }

        public void AddNode(FistrNode node)
        {
            CheckIndex(node.Id);
            Nodes.Add(node);
        }

        private void CheckIndex(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Node id must be greater than 0");
            }

            if (Nodes.Exists(n => n.Id == id))
            {
                throw new ArgumentException($"Node with id {id} already exists");
            }
        }

        public void AddNodeRange(IEnumerable<FistrNode> nodes)
        {
            foreach (FistrNode node in nodes)
            {
                AddNode(node);
            }
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine("!NODE");
            foreach (FistrNode node in Nodes)
            {
                sb.AppendLine(node.ToMsh());
            }
            return sb.ToString();
        }
    }
}
