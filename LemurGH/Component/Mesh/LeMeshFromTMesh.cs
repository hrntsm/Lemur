using System;

using Grasshopper.Kernel;

using Lemur.Mesh;
using Lemur.Mesh.Element;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Mesh
{
    public class LeMeshFromTetraGHMesh : GH_Component
    {
        public LeMeshFromTetraGHMesh()
          : base("LeMeshFromTetraGHMesh", "LeFT",
            "Create Lemur mesh from TetraGH mesh",
            "Lemur", "Mesh")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Header", "Header", "Mesh file header", GH_ParamAccess.item);
            pManager.AddMeshParameter("TetraGHMesh", "TghMesh", "TetraGH mesh object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "Lemur mesh object", GH_ParamAccess.item);
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string header = string.Empty;
            Rhino.Geometry.Mesh tghMesh = null;
            if (!DA.GetData(0, ref header)) return;
            if (!DA.GetData(1, ref tghMesh)) return;

            var leMesh = new LeMesh(header);
            for (int i = 0; i < tghMesh.Vertices.Count; i++)
            {
                Rhino.Geometry.Point3f v = tghMesh.Vertices[i];
                var node = new LeNode(i + 1, v.X, v.Y, v.Z);
                leMesh.AddNode(node);
            }

            int[] connection = tghMesh.UserDictionary["tets"] as int[];
            int elemId = 1;
            for (int i = 0; i < connection.Length; i += 4)
            {
                var element = new Tetra341(elemId, new[] { connection[i] + 1, connection[i + 1] + 1, connection[i + 2] + 1, connection[i + 3] + 1 });
                leMesh.AddElement(element);
                elemId++;
            }

            leMesh.ComputeNodeFaceDataStructure();
            Rhino.Geometry.Mesh mesh = Utils.Preview.LeFaceToRhinoMesh(leMesh, leMesh.FaceMesh);

            Message = $"{leMesh.Nodes.Count} nodes, {leMesh.AllElements.Length} elems";
            DA.SetData(0, new GH_LeMesh(leMesh));
            DA.SetData(1, mesh);
        }

        public override Guid ComponentGuid => new Guid("a7fb6c25-ce8e-46de-afd8-b88ccaba8137");
    }
}
