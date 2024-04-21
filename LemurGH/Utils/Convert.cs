using System;
using System.Collections.Generic;
using System.Linq;

using Lemur.Mesh;
using Lemur.Mesh.Element;

using Rhino.Geometry;

namespace LemurGH.Utils
{
    public static class Convert
    {

        public static LeMesh Tetra341To342(LeMesh baseMesh, Brep brep)
        {
            var nodes = new LeNodeList(baseMesh.Nodes);
            LeElementList tetLinear = baseMesh.Elements.FirstOrDefault(e => e.ElementType == LeElementType.Tetra341);
            Dictionary<int, Tetra342> tetQuadric = Tetra342.CreateFromLinearElementList(tetLinear);
            int nodeMaxId = baseMesh.Nodes.Max(n => n.Id);
            int idOffset = 1;

            foreach (LeEdge edge in baseMesh.Edges)
            {
                LeNode[] edgeNodes = edge.Nodes;
                var edgeNode = new LeNode(nodeMaxId + idOffset++, (edgeNodes[0].X + edgeNodes[1].X) / 2, (edgeNodes[0].Y + edgeNodes[1].Y) / 2, (edgeNodes[0].Z + edgeNodes[1].Z) / 2);
                if (brep != null && edge.IsNaked)
                {
                    var point = new Point3d(edgeNode.X, edgeNode.Y, edgeNode.Z);
                    Point3d closestPoint = brep.ClosestPoint(point);
                    edgeNode = new LeNode(edgeNode.Id, closestPoint.X, closestPoint.Y, closestPoint.Z);
                }
                nodes.Add(edgeNode);

                foreach (int elementId in edge.ElementIds)
                {
                    if (tetQuadric.TryGetValue(elementId, out Tetra342 tetra))
                    {
                        int[] eNodeIds = tetra.NodeIds;
                        int index1 = Array.IndexOf(eNodeIds, edgeNodes[0].Id);
                        int index2 = Array.IndexOf(eNodeIds, edgeNodes[1].Id);

                        tetra.SetNodeId(Tetra342.GetEdgeNodeIndex(index1, index2), edgeNode.Id);
                    }
                }
            }

            var newMesh = new LeMesh(baseMesh.Header);
            newMesh.BuildMesh(nodes, tetQuadric.Values);

            return newMesh;
        }
    }
}
