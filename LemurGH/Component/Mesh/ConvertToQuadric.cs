using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Lemur.Mesh;

using LemurGH.Param;
using LemurGH.Type;

using Rhino.Geometry;

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
            pManager.AddBrepParameter("Brep", "Brep", "Brep object", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh object", GH_ParamAccess.item);
            pManager.AddMeshParameter("SurfaceMesh", "Mesh", "Surface Mesh object", GH_ParamAccess.item);
            pManager.AddLineParameter("Edges", "Edges", "Edge lines", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghLeMesh = new GH_LeMesh();
            Brep brep = null;
            if (!DA.GetData(0, ref ghLeMesh)) return;
            DA.GetData(1, ref brep);

            var baseMesh = new LeMesh(ghLeMesh.Value);
            LeMesh leMesh = Utils.Convert.Tetra341To342(baseMesh, brep);

            Message = $"{leMesh.Nodes.Count} nodes, {leMesh.AllElements.Length} elems";
            Rhino.Geometry.Mesh mesh = Utils.Preview.LeFaceToRhinoMesh(leMesh);
            List<Line> lines = Utils.Preview.LeEdgesToRhinoLines(leMesh);
            DA.SetData(0, new GH_LeMesh(leMesh));
            DA.SetData(1, mesh);
            DA.SetDataList(2, lines);
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("35e877a1-07b9-4e37-8dbd-e810def4aa49");
    }
}
