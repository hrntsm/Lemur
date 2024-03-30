using System;

using Grasshopper.Kernel;

using Lemur;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Util
{
    public class PreviewInput : GH_Component
    {
        public PreviewInput()
          : base("Preview Input", "PrevIn",
              "Preview fistr input items",
              "Lemur", "Util")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeAssemble(), "LeAssemble", "LeAsm", "Lemur Assemble", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Msh", "M", "Mesh", GH_ParamAccess.item);
            pManager.AddTextParameter("Control", "C", "Control", GH_ParamAccess.item);
            pManager.AddTextParameter("Hec", "D", "Hecmw data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeAssemble ghLeAsm = null;
            if (!DA.GetData(0, ref ghLeAsm)) return;
            LeAssemble leAsm = ghLeAsm.Value;

            DA.SetData(0, leAsm.LeMesh.ToMsh());
            DA.SetData(1, leAsm.LeControl.ToCnt());
            DA.SetData(2, leAsm.LeHecmwControl.ToDat());
        }

        public override Guid ComponentGuid => new Guid("3a9315ca-913a-4156-8f79-2b1b426aad6c");
    }
}
