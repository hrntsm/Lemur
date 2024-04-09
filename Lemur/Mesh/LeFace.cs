using System.Collections.Generic;
using System.Linq;

namespace Lemur.Mesh
{
    public class LeFace
    {
        public int Id { get; }
        public LeNode[] Nodes { get; }
        public int[] GetNodeIds() => Nodes.Select(n => n.Id).ToArray();
        public List<(int ElementId, int LocalFaceId)> ElementFaceIds { get; }
        public bool IsSurface => ElementFaceIds.Count == 1;
        public bool IsInternal => ElementFaceIds.Count > 1;

        public LeFace(int id, LeNode[] nodes)
        {
            Id = id;
            Nodes = nodes;
            ElementFaceIds = new List<(int ElementId, int LocalFaceId)>();
        }
    }
}
