using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Iguana.IguanaMesh.ITypes;

using Lemur.Mesh;
using Lemur.Mesh.Element;

using LemurGH.Param;
using LemurGH.Goo;

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

            DA.SetData(0, new GH_LeMesh(leMesh));
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
