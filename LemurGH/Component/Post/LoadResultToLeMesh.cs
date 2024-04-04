using System;

using Grasshopper.Kernel;

using Lemur.Mesh;
using Lemur.Post;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Post
{
    public class LoadResultToLeMesh : GH_Component
    {
        public LoadResultToLeMesh()
          : base("LoadResultToLeMesh", "ResMesh",
            "Load result file to LeMesh",
            "Lemur", "Post")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh object", GH_ParamAccess.item);
            pManager.AddTextParameter("DirPath", "DirPath", "Directory path", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghLeMesh = new GH_LeMesh();
            string dirPath = string.Empty;
            if (!DA.GetData(0, ref ghLeMesh)) return;
            if (!DA.GetData(1, ref dirPath)) return;

            LeMesh leMesh = ghLeMesh.Value;
            _ = new LePost(leMesh, dirPath);

            DA.SetData(0, new GH_LeMesh(leMesh));
        }

        public override Guid ComponentGuid => new Guid("19d6d6af-3038-4967-b298-12d1e9377e42");
    }
}
