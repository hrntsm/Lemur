using System;

using Grasshopper.Kernel;

using Lemur.Mesh;
using Lemur.Mesh.Element;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Mesh
{
    public class ConvertToQuadric : GH_Component
    {
        public ConvertToQuadric()
          : base("ConvertToQuadric", "ConvertToQuadric",
            "Convert Lemur mesh to quadric mesh",
            "Lemur", "Mesh")
        {
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh object", GH_ParamAccess.item);
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghLeMesh = new GH_LeMesh();
            if (!DA.GetData(0, ref ghLeMesh)) return;

            var baseMesh = new LeMesh(ghLeMesh.Value);
            LeMesh leMesh = Tetra342.ConvertFromTetra341(baseMesh);

            Message = $"{leMesh.Nodes.Count} nodes, {leMesh.AllElements.Length} elems";
            Rhino.Geometry.Mesh mesh = Utils.Preview.LeFaceToRhinoMesh(leMesh);
            DA.SetData(0, new GH_LeMesh(leMesh));
            DA.SetData(1, mesh);
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("35e877a1-07b9-4e37-8dbd-e810def4aa49");
    }
}
