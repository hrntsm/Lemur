using System;

using Grasshopper.Kernel;

using Lemur.Control;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Control
{
    public class ConstructLeControl : GH_Component
    {
        public ConstructLeControl()
          : base("ConstructLeControl", "ConLeCnt",
            "Construct Lemur Control settings",
            "Lemur", "Control")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeControl(), "LeCnt", "LeCnt", "Lemur Control settings", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var leCnt = new LeControl();
            DA.SetData(0, new GH_LeControl(leCnt));
        }

        public override Guid ComponentGuid => new Guid("43f47826-30d0-46db-88ee-b4f94789ce81");
    }
}
