using System;

using Grasshopper.Kernel;

using Lemur.Control.Step;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Control
{
    public class ConstructStep : GH_Component
    {
        public ConstructStep()
         : base("ConstructStep", "ConStep",
            "Construct step settings",
            "Lemur", "Control")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Sub Steps", "S", "Number of sub steps", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Convergence", "C", "Convergence", GH_ParamAccess.item, 1e-6);
            pManager.AddIntegerParameter("Max Iterations", "M", "Maximum number of iterations", GH_ParamAccess.item, 100);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeStep(), "Step", "S", "Step", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int subSteps = 1;
            double convergence = 1e-6;
            int maxIter = 100;

            if (!DA.GetData(0, ref subSteps)) return;
            if (!DA.GetData(1, ref convergence)) return;
            if (!DA.GetData(2, ref maxIter)) return;

            var step = new LeStep(subSteps, convergence, maxIter);
            DA.SetData(0, new GH_LeStep(step));
        }

        public override Guid ComponentGuid => new Guid("64f2e18d-31f9-4289-8131-9ee46a96188d");
    }
}
