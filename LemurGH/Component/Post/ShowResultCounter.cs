using System;

using Grasshopper.Kernel;

using Lemur.Mesh;
using Lemur.Post.ColorContour;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Post
{
    public class ShowResultContour : GH_Component
    {
        public ShowResultContour()
          : base("Show Result Contour", "LeContour",
              "Show Result Contour",
              "Lemur", "Post")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Step", "Step", "Step", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("ResultName", "ResultName", "ResultName", GH_ParamAccess.item, "NodalMISES");
            pManager.AddIntegerParameter("ContourSet", "ContourSet", "ContourSet", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Contour", "Contour", "Contour", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeMesh ghLeMesh = null;
            int step = 0;
            string resultName = string.Empty;
            int contourSet = 0;
            if (!DA.GetData(0, ref ghLeMesh)) return;
            if (!DA.GetData(1, ref step)) return;
            if (!DA.GetData(2, ref resultName)) return;
            if (!DA.GetData(3, ref contourSet)) return;

            LeMesh leMesh = ghLeMesh.Value;
            if (leMesh.Nodes[0].NodalResults.Length == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No result data");
                return;
            }

            leMesh.ComputeNodeFaceDataStructure();
            var set = (ContourSet)contourSet;
            Rhino.Geometry.Mesh mesh = Utils.Preview.LeMeshResultToRhinoMesh(leMesh, step, resultName, set);
            Message = $"{resultName} at {step}:{set}";

            DA.SetData(0, mesh);
        }

        public override Guid ComponentGuid => new Guid("3a67d651-72b7-4ec5-8941-c4505023c340");
    }
}
