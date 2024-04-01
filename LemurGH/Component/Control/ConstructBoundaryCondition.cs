using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Lemur.Control;
using Lemur.Control.BoundaryCondition;

using LemurGH.Param;
using LemurGH.Type;

using Rhino.Geometry;

namespace LemurGH.Component.Control
{
    public class ConstructBoundaryCondition : GH_Component
    {
        public ConstructBoundaryCondition()
          : base("ConstructBoundaryCondition", "ConBC",
            "Construct Lemur Boundary Condition settings",
            "Lemur", "Control")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("TargetGroupName", "TgtGrp", "Target group name", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Type", "Type", "0:Disp, 1:Spring, 2:CLOAD, 3:DLOAD", GH_ParamAccess.item, 0);
            pManager.AddVectorParameter("Values", "Values", "Values", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Constraints", "Constraints", "Constraints", GH_ParamAccess.list, new List<bool>() { true, true, true });
            Params.Input[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeBC(), "LeBC", "LeBC", "Lemur Boundary Condition settings", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string targetGroupName = string.Empty;
            int type = 0;
            var vector = new Vector3d();
            var constraints = new List<bool>();

            if (!DA.GetData(0, ref targetGroupName)) return;
            if (!DA.GetData(1, ref type)) return;
            if (!DA.GetData(2, ref vector)) return;
            DA.GetDataList(3, constraints);

            double[] values = new double[] { vector.X, vector.Y, vector.Z };
            var leBC = new LeBoundaryCondition(targetGroupName, (LeBCType)type, values, constraints.ToArray());

            DA.SetData(0, new GH_LeBC(leBC));
        }

        public override Guid ComponentGuid => new Guid("354efd75-e259-4112-83df-ea863c091b0a");
    }
}
