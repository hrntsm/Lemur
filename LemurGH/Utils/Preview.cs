using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

using Lemur.Mesh;
using Lemur.Mesh.Element;
using Lemur.Post.ColorContour;
using Lemur.Post.Mesh;

using Rhino.Geometry;

namespace LemurGH.Utils
{
    public static class Preview
    {
        public static Mesh LeFaceToRhinoMesh(LeMesh leMesh)
        {
            IEnumerable<LeFace> face = leMesh.Faces.Where(f => f.IsSurface);
            return LeFaceToRhinoMesh(leMesh, face.Select(f => f.ElementFaceIds).SelectMany(f => f).ToArray());
        }

        public static Mesh LeFaceToRhinoMesh(LeMesh leMesh, (int elementId, int faceId)[] faceIds)
        {
            var rhinoMesh = new Mesh();
            rhinoMesh.Vertices.AddVertices(leMesh.Nodes.Select(n => new Point3d(n.X, n.Y, n.Z)));

            foreach ((int elementId, int faceId) in faceIds)
            {
                LeSolidElementBase solid = leMesh.AllElements.OfType<LeSolidElementBase>().FirstOrDefault(elem => elem.Id == elementId);
                int[] nodes = solid?.FaceToNodes(faceId);
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

        public static (Mesh, Color[], string[]) LeMeshResultToRhinoMesh(LeMesh leMesh, int step, string resultName, ContourSet contourSet, double scale)
        {
            LeSolidElementBase[] solids = leMesh.AllElements.OfType<LeSolidElementBase>().ToArray();
            (double min, double max) = leMesh.NodalResultSummary[step][resultName];

            var rhinoMesh = new Mesh();

            IEnumerable<LeNode> deformedNodes = leMesh.Nodes.Select(n => n.GetDeformedNode(step, scale));
            rhinoMesh.Vertices.AddVertices(deformedNodes.Select(n => new Point3d(n.X, n.Y, n.Z)));

            foreach (LeNode node in leMesh.Nodes)
            {
                if (node.NodalResults.Length == 0)
                {
                    continue;
                }
                LeNodalResult result = node.NodalResults.FirstOrDefault(r => r.StepNumber == step);
                if (result == null)
                {
                    continue;
                }
                if (!result.NodalData.ContainsKey(resultName))
                {
                    continue;
                }
                double[] data = result.NodalData[resultName];
                double norm = Math.Sqrt(data.Sum(d => d * d));
                double normalizedValue = (norm - min) / (max - min);
                if (double.IsNaN(normalizedValue))
                {
                    normalizedValue = 0;
                }

                var contouring = new Contouring(ContoursData.ColorContours[contourSet.ToString()]);
                Color color = contouring.GetColor(normalizedValue);
                rhinoMesh.VertexColors.Add(color);
            }

            foreach ((int elementId, int faceId) in leMesh.Faces.Where(f => f.IsSurface).Select(f => f.ElementFaceIds.First()))
            {
                LeSolidElementBase solid = solids.FirstOrDefault(elem => elem.Id == elementId);
                int[] nodes = solid?.FaceToNodes(faceId);
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

            (Color color, double position)[] colorContour = ContoursData.ColorContours[contourSet.ToString()];
            Color[] colors = colorContour.Select(c => c.color).Reverse().ToArray();
            string[] texts = colorContour.Select(c => (c.position * (max - min) - min).ToString("0.0000E+00", CultureInfo.InvariantCulture)).Reverse().ToArray();
            return (rhinoMesh, colors, texts);
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
