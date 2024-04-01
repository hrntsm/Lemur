using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using Lemur.Control;
using Lemur.Control.BoundaryCondition;

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
            pManager.AddParameter(new Param_LeBC(), "BoundaryConditions", "BC", "Boundary Conditions", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeControl(), "LeCnt", "LeCnt", "Lemur Control settings", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghBCList = new List<GH_LeBC>();
            if (!DA.GetDataList(0, ghBCList)) return;

            var leBCList = ghBCList.Select(x => x.Value).ToList();

            var leCnt = new LeControl();
            foreach (LeBoundaryCondition leBC in leBCList)
            {
                leCnt.AddBC(leBC);
            }

            DA.SetData(0, new GH_LeControl(leCnt));
        }

        public override Guid ComponentGuid => new Guid("43f47826-30d0-46db-88ee-b4f94789ce81");
    }
}
