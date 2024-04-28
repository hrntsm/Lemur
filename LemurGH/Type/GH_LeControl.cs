using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Control;
using Lemur.Control.BoundaryCondition;
using Lemur.Control.Output;
using Lemur.Section;


namespace LemurGH.Type
{
    public class GH_LeControl : GH_Goo<LeControl>
    {
        public GH_LeControl()
        {
        }

        public GH_LeControl(LeControl leControl)
         : base(leControl)
        {
        }

        public GH_LeControl(GH_LeControl leControl)
         : base(leControl.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeControl";
        public override string TypeDescription => "Lemur Control settings";
        public override IGH_GooProxy EmitProxy() => new GH_LeControlProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeControl(Value);
        public override bool CastTo<T>(ref T target)
        {
            if (typeof(LeControl).IsAssignableFrom(typeof(T)))
            {
                target = (T)(object)new LeControl(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeControl leControl)
            {
                Value = leControl;
                return true;
            }
            else
            {
                return base.CastFrom(source);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"LeControl:");
            sb.AppendLine($"- SolutionType: {Value.SolutionType}");
            sb.AppendLine($"- LeWrites:");
            foreach (LeWrite leWrite in Value.LeWrites)
            {
                sb.AppendLine($"  - {leWrite.LeWriteType}");
            }
            sb.AppendLine($"- LeSection:");
            foreach (LeSection leSection in Value.LeSection)
            {
                sb.AppendLine($"  - Id:{leSection.Id}");
                sb.AppendLine($"  - Material:{leSection.Material.Name}");
            }
            sb.AppendLine($"- LeBC:");
            foreach (LeBoundaryCondition leBC in Value.LeBoundaryConditions)
            {
                sb.AppendLine($"  - TargetGroup:{leBC.TargetGroupName}, Type:{leBC.Type}");
            }
            if (Value.LeContactControl != null)
            {
                sb.AppendLine($"- LeContactControl");
                sb.AppendLine($"  - {Value.LeContactControl.Algorithm}, {Value.LeContactControl.Interaction}, Pair:{Value.LeContactControl.TargetContactPair}");
            }
            sb.AppendLine($"- LeStep:");
            sb.AppendLine($"  - SubSteps: {Value.LeStep.SubSteps}, MaxIter: {Value.LeStep.MaxIter}, Converg: {Value.LeStep.Convergence}");
            sb.AppendLine($"- LeSolver:");
            sb.AppendLine($"  - {Value.LeSolver.Method}, {Value.LeSolver.Precondition}, {Value.LeSolver.MaxIter}, {Value.LeSolver.Residual}");

            return sb.ToString();
        }

        public class GH_LeControlProxy : GH_GooProxy<GH_LeControl>
        {
            public GH_LeControlProxy(GH_LeControl owner)
             : base(owner)
            {
            }
        }
    }
}
