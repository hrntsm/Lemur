using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using Iguana.IguanaMesh.ITypes;

using Lemur.Mesh;
using Lemur.Mesh.Element;

using LemurGH.Param;
using LemurGH.Type;

using Rhino.Geometry;

namespace LemurGH.Component
{
    public class LeMeshFromIMesh : GH_Component
    {
        public LeMeshFromIMesh()
          : base("LeMeshFromIMesh", "LeFI",
            "Create Lemur mesh from Iguana mesh",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Header", "Header", "Mesh file header", GH_ParamAccess.item);
            pManager.AddGenericParameter("iM", "iMesh", "iMesh object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "Lemur mesh object", GH_ParamAccess.item);
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string header = string.Empty;
            object iMInput = null;
            if (!DA.GetData(0, ref header)) return;
            if (!DA.GetData(1, ref iMInput)) return;

            var leMesh = new LeMesh(header);

            if (!(iMInput is IMesh iMesh)) return;

            ConvertINodeToFNode(leMesh, iMesh);
            ConvertIElementToFElement(leMesh, iMesh);

            Mesh mesh = GetFaceMesh(leMesh);

            DA.SetData(0, new GH_LeMesh(leMesh));
            DA.SetData(1, mesh);
        }

        private static Mesh GetFaceMesh(LeMesh leMesh)
        {
            (int, int)[] aa = leMesh.FaceMesh;
            var elements = new List<LeElementBase>();
            foreach (LeElementList elementList in leMesh.Elements)
            {
                elements.AddRange(elementList);
            }

            var mesh = new Mesh();
            mesh.Vertices.AddVertices(leMesh.Nodes.Select(n => new Point3d(n.X, n.Y, n.Z)));
            foreach ((int, int) a in aa)
            {
                LeElementBase e = elements.FirstOrDefault(elem => elem.Id == a.Item1);
                int[] nodes = e?.GetSurfaceNodesFromId(a.Item2);
                if (nodes != null)
                {
                    if (nodes.Length == 3)
                        mesh.Faces.AddFace(new MeshFace(nodes[0] - 1, nodes[1] - 1, nodes[2] - 1));
                    else if (nodes.Length == 4)
                        mesh.Faces.AddFace(new MeshFace(nodes[0] - 1, nodes[1] - 1, nodes[2] - 1, nodes[3] - 1));
                }
            }
            mesh.UnifyNormals();
            mesh.Normals.ComputeNormals();
            return mesh;
        }

        private static void ConvertINodeToFNode(LeMesh leMesh, IMesh iMesh)
        {
            List<ITopologicVertex> vertices = iMesh.Vertices;
            foreach (ITopologicVertex vertex in vertices)
            {
                var node = new LeNode(vertex.Key, vertex.X, vertex.Y, vertex.Z);
                leMesh.AddNode(node);
            }
        }

        private static void ConvertIElementToFElement(LeMesh leMesh, IMesh iMesh)
        {
            List<IElement> elements = iMesh.Elements;
            foreach (IElement element in elements)
            {
                switch (element)
                {
                    case ITetrahedronElement iTetra:
                        leMesh.AddElement(Tetra341.FromIguanaElement(iTetra));
                        break;
                }
            }

        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("26ee9dc0-a3fc-4f4c-a1aa-f200f1f58411");
    }
}
