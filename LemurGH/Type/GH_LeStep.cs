using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Control.Step;


namespace LemurGH.Type
{
    public class GH_LeStep : GH_Goo<LeStep>
    {
        public GH_LeStep()
        {
        }

        public GH_LeStep(LeStep leStep)
         : base(leStep)
        {
        }

        public GH_LeStep(GH_LeStep other)
         : base(other.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeStep";
        public override string TypeDescription => "Lemur Step";
        public override IGH_GooProxy EmitProxy() => new GH_LeStepProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeStep(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeStep).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)new LeStep(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeStep leStep)
            {
                Value = leStep;
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
            sb.AppendLine($"LeStep:");
            sb.AppendLine($"  SubSteps: {Value.SubSteps}");
            sb.AppendLine($"  MaxIterations: {Value.MaxIter}");
            sb.AppendLine($"  Convergence: {Value.Convergence}");
            return sb.ToString();
        }

        public class GH_LeStepProxy : GH_GooProxy<GH_LeStep>
        {
            public GH_LeStepProxy(GH_LeStep owner) : base(owner) { }
        }
    }
}
