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

namespace LemurGH.Component.Mesh
{
    public class LeMeshFromIMesh : GH_Component
    {
        public LeMeshFromIMesh()
          : base("LeMeshFromIMesh", "LeFI",
            "Create Lemur mesh from Iguana mesh",
            "Lemur", "Mesh")
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
            pManager.AddLineParameter("Edges", "Edges", "Mesh edges", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string header = string.Empty;
            object iMInput = null;
            if (!DA.GetData(0, ref header)) return;
            if (!DA.GetData(1, ref iMInput)) return;

            var leMesh = new LeMesh(header);

            if (!(iMInput is IMesh iMesh)) return;

            LeNode[] nodes = ConvertINodeToLeNode(iMesh);
            LeElementBase[] elems = ConvertIElementToLeElement(iMesh);
            leMesh.BuildMesh(nodes, elems);

            Rhino.Geometry.Mesh mesh = Utils.Preview.LeFaceToRhinoMesh(leMesh);
            List<Line> edges = Utils.Preview.LeEdgesToRhinoLines(leMesh);

            Message = $"{leMesh.Nodes.Count} nodes, {leMesh.AllElements.Length} elems";
            DA.SetData(0, new GH_LeMesh(leMesh));
            DA.SetData(1, mesh);
            DA.SetDataList(2, edges);
        }

        private static LeNode[] ConvertINodeToLeNode(IMesh iMesh)
        {
            var nodes = new HashSet<LeNode>();
            List<ITopologicVertex> vertices = iMesh.Vertices;
            foreach (ITopologicVertex vertex in vertices)
            {
                var node = new LeNode(vertex.Key, vertex.X, vertex.Y, vertex.Z);
                nodes.Add(node);
            }
            return nodes.ToArray();
        }

        private static LeElementBase[] ConvertIElementToLeElement(IMesh iMesh)
        {
            var leElems = new HashSet<LeElementBase>();
            List<IElement> iElems = iMesh.Elements;
            int idOffset = 0;
            foreach (IElement iElem in iElems)
            {
                switch (iElem)
                {
                    case ITetrahedronElement iTetra:
                        leElems.Add(Tetra341.FromIguanaElement(iTetra, idOffset));
                        break;
                    case IPrismElement iPrism:
                        leElems.Add(Prism351.FromIguanaElement(iPrism, idOffset));
                        break;
                    case IHexahedronElement iHexa:
                        leElems.Add(Hexa361.FromIguanaElement(iHexa, idOffset));
                        break;
                    case IPyramidElement iPyramid:
                        Tetra341[] elems = Tetra341.FromIguanaElement(iPyramid, idOffset);
                        for (int i = 0; i < elems.Length; i++)
                        {
                            leElems.Add(elems[i]);
                        }
                        idOffset++;
                        break;
                    default:
                        throw new NotImplementedException($"Element type {iElem.GetType()} is not implemented.");
                }
            }
            return leElems.ToArray();
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("26ee9dc0-a3fc-4f4c-a1aa-f200f1f58411");
    }
}
