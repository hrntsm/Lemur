using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using Lemur.Control;
using Lemur.Control.BoundaryCondition;
using Lemur.Control.Solution;
using Lemur.Control.Solver;

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
            pManager.AddIntegerParameter("SolutionType", "SolType", "0:STATIC, 1:NLSTATIC, 3:EIGEN", GH_ParamAccess.item, 0);
            pManager.AddParameter(new Param_LeBC(), "BoundaryConditions", "BC", "Boundary Conditions", GH_ParamAccess.list);
            pManager.AddParameter(new Param_LeSolver(), "Solver", "Solver", "Solver", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeControl(), "LeCnt", "LeCnt", "Lemur Control settings", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int solType = 0;
            var ghBCList = new List<GH_LeBC>();
            var ghSolver = new GH_LeSolver();
            if (!DA.GetData(0, ref solType)) return;
            if (!DA.GetDataList(1, ghBCList)) return;
            if (!DA.GetData(2, ref ghSolver)) return;

            var leBCList = ghBCList.Select(x => x.Value).ToList();
            LeSolver leSolver = ghSolver.Value;

            var leCnt = new LeControl((LeSolutionType)solType, leSolver);
            foreach (LeBoundaryCondition leBC in leBCList)
            {
                leCnt.AddBC(leBC);
            }

            DA.SetData(0, new GH_LeControl(leCnt));
        }

        public override Guid ComponentGuid => new Guid("43f47826-30d0-46db-88ee-b4f94789ce81");
    }
}
