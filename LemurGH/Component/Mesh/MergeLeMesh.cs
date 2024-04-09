using System;

using Grasshopper.Kernel;

using Lemur.Mesh;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Mesh
{
    public class MergeLeMesh : GH_Component
    {
        public MergeLeMesh()
          : base("MergeLeMesh", "MergeLeMesh",
            "Merge Lemur mesh A into B",
            "Lemur", "Mesh")
        {
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh1", "LeM1", "LeMesh object", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeMesh(), "LeMesh2", "LeM2", "LeMesh object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh object", GH_ParamAccess.item);
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghLeMesh1 = new GH_LeMesh();
            var ghLeMesh2 = new GH_LeMesh();
            if (!DA.GetData(0, ref ghLeMesh1)) return;
            if (!DA.GetData(1, ref ghLeMesh2)) return;

            LeMesh leMesh1 = ghLeMesh1.Value;
            LeMesh leMesh2 = ghLeMesh2.Value;

            leMesh1.Merge(leMesh2);

            Message = $"{leMesh1.Nodes.Count} nodes, {leMesh1.AllElements.Length} elems";
            Rhino.Geometry.Mesh mesh = Utils.Preview.LeFaceToRhinoMesh(leMesh1);
            DA.SetData(0, new GH_LeMesh(leMesh1));
            DA.SetData(1, mesh);
        }

        public override Guid ComponentGuid => new Guid("35e877a1-07b9-4e37-8dbd-e810def4aa48");
    }
}
