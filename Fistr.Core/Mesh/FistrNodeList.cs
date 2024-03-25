using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fistr.Core.Mesh
{
    public class FistrNodeList : IList<FistrNode>
    {
        public bool IsReadOnly => true;

        public int Count => _nodes.Count;

        public FistrNode this[int index]
        {
            get => _nodes[index];
            set => _nodes[index] = value;
        }

        private readonly List<FistrNode> _nodes;

        public FistrNodeList()
        {
            _nodes = new List<FistrNode>();
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

        public IEnumerator<FistrNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(FistrNode item)
        {
            return _nodes.IndexOf(item);
        }

        public void Insert(int index, FistrNode item)
        {
            _nodes.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _nodes.RemoveAt(index);
        }

        public void Add(FistrNode item)
        {
            CheckIndex(item.Id);
            _nodes.Add(item);
        }

        public void Add(double x, double y, double z)
        {
            int id = _nodes.Max(n => n.Id) + 1;
            Add(id, x, y, z);
        }

        public void Add(int id, double x, double y, double z)
        {
            var node = new FistrNode(id, x, y, z);
            Add(node);
        }

        public void AddRange(IEnumerable<FistrNode> nodes)
        {
            foreach (FistrNode node in nodes)
            {
                Add(node);
            }
        }

        public void Clear()
        {
            _nodes.Clear();
        }

        public bool Contains(FistrNode item)
        {
            return _nodes.Contains(item);
        }

        public void CopyTo(FistrNode[] array, int arrayIndex)
        {
            _nodes.CopyTo(array, arrayIndex);
        }

        public bool Remove(FistrNode item)
        {
            return _nodes.Remove(item);
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
