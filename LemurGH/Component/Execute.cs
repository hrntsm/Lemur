using System;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Lemur.Mesh;

namespace LemurGH.Component
{
    public class Execute : GH_Component
    {
        public Execute()
          : base("Execute", "Execute",
            "Execute Lemur component",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Exe", "Exe", "Execute analysis", GH_ParamAccess.item);
            pManager.AddGenericParameter("LeMesh", "LeMesh", "Input LeMesh", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "Path", "Path to save results", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result of execution", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool exe = false;
            object leMeshInput = null;
            string path = string.Empty;
            if (!DA.GetData(0, ref exe)) return;
            if (!DA.GetData(1, ref leMeshInput)) return;
            if (!DA.GetData(2, ref path)) return;

            var leMeshObj = (GH_ObjectWrapper)leMeshInput;
            var leMesh = (LeMesh)leMeshObj.Value;
            leMesh?.Serialize(path);
        }

        public override Guid ComponentGuid => new Guid("20aab5a3-c180-4a1f-806b-4b3c16cc8f28");
    }
}
