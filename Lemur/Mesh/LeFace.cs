using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemur.Mesh
{
    [Serializable]
    public class LeFace
    {
        public int Id { get; }
        public LeNode[] Nodes { get; }
        public int[] GetNodeIds() => Nodes.Select(n => n.Id).ToArray();
        public HashSet<(int ElementId, int LocalFaceId)> ElementFaceIds { get; }
        public HashSet<LeEdge> Edges { get; }
        public bool IsSurface => ElementFaceIds.Count == 1;
        public bool IsInternal => ElementFaceIds.Count > 1;

        public LeFace(int id, LeNode[] nodes)
        {
            Id = id;
            Nodes = nodes;
            ElementFaceIds = new HashSet<(int ElementId, int LocalFaceId)>();
            Edges = new HashSet<LeEdge>();
        }

        public Dictionary<int, int[]> GetEdgeIds()
        {
            var edgeIds = new Dictionary<int, int[]>();
            int[] nodeIds = GetNodeIds();
            Array.Sort(nodeIds);
            switch (nodeIds.Length)
            {
                case 3:
                    edgeIds.Add(0, new[] { nodeIds[0], nodeIds[1] });
                    edgeIds.Add(1, new[] { nodeIds[0], nodeIds[2] });
                    edgeIds.Add(2, new[] { nodeIds[1], nodeIds[2] });
                    break;
                case 4:
                    edgeIds.Add(0, new[] { nodeIds[0], nodeIds[1] });
                    edgeIds.Add(1, new[] { nodeIds[0], nodeIds[3] });
                    edgeIds.Add(2, new[] { nodeIds[1], nodeIds[2] });
                    edgeIds.Add(3, new[] { nodeIds[2], nodeIds[3] });
                    break;
            }
            return edgeIds;
        }
    }
}
