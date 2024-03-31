using System.Linq;

using Lemur.Mesh;
using Lemur.Mesh.Element;

using Rhino.Geometry;

namespace LemurGH.Utils
{
    public static class Preview
    {
        public static Mesh LeFaceToRhinoMesh(LeMesh leMesh, (int, int)[] faceNodes)
        {
            LeSolidElementBase[] solids = leMesh.AllElements.OfType<LeSolidElementBase>().ToArray();

            var rhinoMesh = new Mesh();
            rhinoMesh.Vertices.AddVertices(leMesh.Nodes.Select(n => new Point3d(n.X, n.Y, n.Z)));
            foreach ((int, int) faceNode in faceNodes)
            {
                LeSolidElementBase solid = solids.FirstOrDefault(elem => elem.Id == faceNode.Item1);
                int[] nodes = solid?.FaceToNodes(faceNode.Item2);
                if (nodes != null)
                {
                    if (nodes.Length == 3)
                        rhinoMesh.Faces.AddFace(new MeshFace(nodes[0] - 1, nodes[1] - 1, nodes[2] - 1));
                    else if (nodes.Length == 4)
                        rhinoMesh.Faces.AddFace(new MeshFace(nodes[0] - 1, nodes[1] - 1, nodes[2] - 1, nodes[3] - 1));
                }
            }
            rhinoMesh.UnifyNormals();
            rhinoMesh.Normals.ComputeNormals();
            return rhinoMesh;
        }

        public static PointCloud LeNodeToRhinoPointCloud(LeMesh leMesh, int[] nodeIds)
        {
            var pointCloud = new PointCloud();
            LeNodeList nodes = leMesh.Nodes;
            foreach (int nodeId in nodeIds)
            {
                LeNode node = nodes.FirstOrDefault(n => n.Id == nodeId);
                if (node != null)
                {
                    pointCloud.Add(new Point3d(node.X, node.Y, node.Z));
                }
            }

            return pointCloud;
        }
    }
}
