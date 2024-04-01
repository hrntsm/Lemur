using System;

using Grasshopper.Kernel;

using Lemur.Control.Solver;

using LemurGH.Type;

namespace LemurGH.Component.Control
{
    public class ConstructSolverSettings : GH_Component
    {
        private readonly int[] _methods = new int[] { 0, 1, 2, 3, 10, 11, 12 };
        private readonly int[] _preconditions = new int[] { 1, 3, 5, 10, 11, 12 };

        public ConstructSolverSettings()
          : base("ConstructSolverSettings", "ConSS",
              "Construct Solver settings",
              "Lemur", "Control")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Method", "Method", "0:CG, 1:BiCGSTAB, 2:GMRES, 3:GPBiCG, 10:DIRECT, 11:DIRECTmkl, 12:MUMPS", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Precondition", "Precond", "1:SSOR, 3:DiagScaling, 5:ML_AMG, 1i:BLOCK_ILUi(0<=I<=2)", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("MaxIter", "MIter", "Maximum number of iterations", GH_ParamAccess.item, 100);
            pManager.AddNumberParameter("Residual", "Residual", "Residual", GH_ParamAccess.item, 1e-8);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param.Param_LeSolver(), "LeSolver", "LeSS", "Lemur Solver settings", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int method = 0;
            int precond = 1;
            int maxIter = 100;
            double residual = 1e-8;
            if (!DA.GetData(0, ref method)) return;
            if (!DA.GetData(1, ref precond)) return;
            if (!DA.GetData(2, ref maxIter)) return;
            if (!DA.GetData(3, ref residual)) return;

            if (Array.IndexOf(_methods, method) < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid method");
                return;
            }
            if (Array.IndexOf(_preconditions, precond) < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid precondition");
                return;
            }

            var solver = new LeSolver((LeSolverMethod)method, (LePrecondition)precond, maxIter, residual);
            DA.SetData(0, new GH_LeSolver(solver));
            Message = $"{(LeSolverMethod)method}, {(LePrecondition)precond}";
        }

        public override Guid ComponentGuid => new Guid("1408200f-99cb-4ef4-89ab-775fdc13fc45");
    }
}
