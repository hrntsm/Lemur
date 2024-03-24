using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fistr.Core.Mesh
{
    public class FistrNodeList
    {
        public FistrNode[] Nodes
        {
            get
            {
                return _nodes.ToArray();
            }
        }

        private readonly List<FistrNode> _nodes;

        public FistrNodeList()
        {
            _nodes = new List<FistrNode>();
        }

        public void AddNode(double x, double y, double z)
        {
            int id = _nodes.Max(n => n.Id) + 1;
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
            _nodes.Add(node);
        }

        private void CheckIndex(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Node id must be greater than 0");
            }

            if (_nodes.Exists(n => n.Id == id))
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
            sb.AppendLine("!NODE, NGRP=NGRP_AUTO");
            foreach (FistrNode node in _nodes)
            {
                sb.AppendLine(node.ToMsh());
            }
            return sb.ToString();
        }
    }
}
