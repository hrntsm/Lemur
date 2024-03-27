using System;
using System.Collections.Generic;

using Fistr.Core.Mesh;
using Fistr.Core.Mesh.Element;

using Grasshopper.Kernel;

using Iguana.IguanaMesh.ITypes;

namespace Fistr.GH.Component
{
    public class FMeshFromIMesh : GH_Component
    {
        public FMeshFromIMesh()
          : base("FMeshFromIMesh", "FFI",
            "Create Fistr mesh from Iguana mesh",
            "FistrGH", "FistrGH")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Header", "Header", "Mesh file header", GH_ParamAccess.item);
            pManager.AddGenericParameter("iM", "iMesh", "iMesh object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string header = string.Empty;
            object iMInput = null;
            if (!DA.GetData(0, ref header)) return;
            if (!DA.GetData(1, ref iMInput)) return;

            var fMesh = new FistrMesh(header);

            if (!(iMInput is IMesh iMesh)) return;

            ConvertINodeToFNode(fMesh, iMesh);
            ConvertIElementToFElement(fMesh, iMesh);

        }

        private static void ConvertINodeToFNode(FistrMesh fMesh, IMesh iMesh)
        {
            List<ITopologicVertex> vertices = iMesh.Vertices;
            foreach (ITopologicVertex vertex in vertices)
            {
                var node = new FistrNode(vertex.Key, vertex.X, vertex.Y, vertex.Z);
                fMesh.AddNode(node);
            }
        }

        private static void ConvertIElementToFElement(FistrMesh fMesh, IMesh iMesh)
        {
            List<IElement> elements = iMesh.Elements;
            foreach (IElement element in elements)
            {
                switch (element)
                {
                    case ITetrahedronElement iTetra:
                        fMesh.AddElement(Tetra341.FromIguanaElement(iTetra));
                        break;
                }
            }

        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("26ee9dc0-a3fc-4f4c-a1aa-f200f1f58411");
    }
}
