using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Lemur.Mesh;
using Lemur.Mesh.Element;

using LemurGH.Param;
using LemurGH.Type;

using Rhino.Geometry;

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
            pManager.AddLineParameter("Edges", "Edges", "Mesh edges", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string header = string.Empty;
            Rhino.Geometry.Mesh tghMesh = null;
            if (!DA.GetData(0, ref header)) return;
            if (!DA.GetData(1, ref tghMesh)) return;

            var leMesh = new LeMesh(header);
            var nodes = new LeNode[tghMesh.Vertices.Count];
            for (int i = 0; i < tghMesh.Vertices.Count; i++)
            {
                Rhino.Geometry.Point3f v = tghMesh.Vertices[i];
                var node = new LeNode(i + 1, v.X, v.Y, v.Z);
                nodes[i] = node;
            }

            var elems = new List<LeElementBase>();
            int[] connection = tghMesh.UserDictionary["tets"] as int[];
            int elemId = 1;
            for (int i = 0; i < connection.Length; i += 4)
            {
                var element = new Tetra341(elemId, new[] { connection[i] + 1, connection[i + 1] + 1, connection[i + 2] + 1, connection[i + 3] + 1 });
                elems.Add(element);
                elemId++;
            }

            leMesh.BuildMesh(nodes, elems.ToArray());
            Rhino.Geometry.Mesh mesh = Utils.Preview.LeFaceToRhinoMesh(leMesh);
            List<Line> edges = Utils.Preview.LeEdgesToRhinoLines(leMesh);

            Message = $"{leMesh.Nodes.Count} nodes, {leMesh.AllElements.Length} elems";
            DA.SetData(0, new GH_LeMesh(leMesh));
            DA.SetData(1, mesh);
            DA.SetDataList(2, edges);
        }

        public override Guid ComponentGuid => new Guid("a7fb6c25-ce8e-46de-afd8-b88ccaba8137");
    }
}
