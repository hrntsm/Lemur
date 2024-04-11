using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemur.Mesh
{
    public class LeNodeList : IList<LeNode>
    {
        public bool IsReadOnly => true;

        public int Count => _nodes.Count;

        public LeNode this[int index]
        {
            get => _nodes[index];
            set => _nodes[index] = value;
        }

        private readonly List<LeNode> _nodes;

        public LeNodeList()
        {
            _nodes = new List<LeNode>();
        }

        public LeNodeList(IEnumerable<LeNode> nodes)
        {
            _nodes = new List<LeNode>(nodes);
        }

        public LeNodeList(LeNodeList other)
        {
            _nodes = new List<LeNode>(other._nodes);
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

        public IEnumerator<LeNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(LeNode item)
        {
            return _nodes.IndexOf(item);
        }

        public void Insert(int index, LeNode item)
        {
            _nodes.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _nodes.RemoveAt(index);
        }

        public void Add(LeNode item)
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
            var node = new LeNode(id, x, y, z);
            Add(node);
        }

        public void AddRange(IEnumerable<LeNode> nodes)
        {
            foreach (LeNode node in nodes)
            {
                Add(node);
            }
        }

        public void Clear()
        {
            _nodes.Clear();
        }

        public bool Contains(LeNode item)
        {
            return _nodes.Contains(item);
        }

        public void CopyTo(LeNode[] array, int arrayIndex)
        {
            _nodes.CopyTo(array, arrayIndex);
        }

        public bool Remove(LeNode item)
        {
            return _nodes.Remove(item);
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine("!NODE, NGRP=NGRP_AUTO");
            foreach (LeNode node in _nodes)
            {
                sb.AppendLine(node.ToMsh());
            }
            return sb.ToString();
        }
    }
}
