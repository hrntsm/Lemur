using System;
using System.Drawing;

using Grasshopper.Kernel;

using Lemur.Mesh;
using Lemur.Post.ColorContour;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Post
{
    public class ResultContour : GH_Component
    {
        public ResultContour()
          : base("ResultContour", "LeContour",
              "Result Contour",
              "Lemur", "Post")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Step", "Step", "Step", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("ResultName", "ResultName", "ResultName", GH_ParamAccess.item, "NodalMISES");
            pManager.AddIntegerParameter("ContourSet", "ContourSet", "ContourSet", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("DispScale", "Scale", "Scale", GH_ParamAccess.item, 1.0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "Color", GH_ParamAccess.list);
            pManager.AddTextParameter("Text", "T", "Text", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeMesh ghLeMesh = null;
            int step = 0;
            string resultName = string.Empty;
            int contourSet = 0;
            double scale = 1.0;
            if (!DA.GetData(0, ref ghLeMesh)) return;
            if (!DA.GetData(1, ref step)) return;
            if (!DA.GetData(2, ref resultName)) return;
            if (!DA.GetData(3, ref contourSet)) return;
            if (!DA.GetData(4, ref scale)) return;

            LeMesh leMesh = ghLeMesh.Value;
            if (leMesh.Nodes[0].NodalResults.Length == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No result data");
                return;
            }

            leMesh.ComputeNodeFaceDataStructure();
            var set = (ContourSet)contourSet;
            (Rhino.Geometry.Mesh mesh, Color[] color, string[] text) = Utils.Preview.LeMeshResultToRhinoMesh(leMesh, step, resultName, set, scale);
            Message = $"{resultName} at {step}:{set}";

            DA.SetData(0, mesh);
            DA.SetDataList(1, color);
            DA.SetDataList(2, text);
        }

        public override Guid ComponentGuid => new Guid("3a67d651-72b7-4ec5-8941-c4505023c340");
    }
}
