using System.Collections.Generic;
using System.Linq;

namespace Lemur.Mesh
{
    public class LeEdge
    {
        public int Id { get; }
        public LeNode[] Nodes { get; }
        public int[] GetNodeIds() => Nodes.Select(n => n.Id).ToArray();
        public HashSet<int> ElementIds { get; }

        public LeEdge(int id, LeNode[] nodes)
        {
            Id = id;
            Nodes = nodes;
            ElementIds = new HashSet<int>();
        }
    }
}
