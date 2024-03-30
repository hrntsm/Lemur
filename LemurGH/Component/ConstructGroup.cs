using System;

using Grasshopper.Kernel;

namespace LemurGH.Component
{
    public class ConstructGroup : GH_Component
    {
        public ConstructGroup()
          : base("ConstructGroup", "ConGrp",
            "Construct Lemur Group",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Group name", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override Guid ComponentGuid => new Guid("4d94af90-ebab-4f0e-a601-8f6d468eb240");
    }
}
