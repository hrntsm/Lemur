using System;

using Grasshopper.Kernel;

using Lemur;
using Lemur.Control;
using Lemur.Mesh;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component
{
    public class ConstructLeAssemble : GH_Component
    {
        public ConstructLeAssemble()
          : base("ConstructLeAssemble", "ConLeAsm",
            "Construct Lemur Assemble",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "Input Lemur Mesh", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeControl(), "LeCnt", "LeCnt", "Input Lemur Control settings", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeAssemble(), "LeAsm", "LeAsm", "Lemur Assemble", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeMesh ghLeMesh = null;
            GH_LeControl ghLeCnt = null;
            if (!DA.GetData(0, ref ghLeMesh)) return;
            if (!DA.GetData(1, ref ghLeCnt)) return;

            LeMesh leMesh = ghLeMesh.Value;
            LeControl leCnt = ghLeCnt.Value;

            var leHecmwControl = new LeHecmwControl();

            var leAsm = new LeAssemble(leMesh, leCnt, leHecmwControl);
            DA.SetData(0, new GH_LeAssemble(leAsm));
        }

        public override Guid ComponentGuid => new Guid("e56e1bc6-a56a-4978-8575-527cf6cec17f");
    }
}
