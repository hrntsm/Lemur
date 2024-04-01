using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Control;
using Lemur.Control.BoundaryCondition;

namespace LemurGH.Type
{
    public class GH_LeBC : GH_Goo<LeBoundaryCondition>
    {
        public GH_LeBC()
        {
        }

        public GH_LeBC(LeBoundaryCondition leBC)
         : base(leBC)
        {
        }

        public GH_LeBC(GH_LeBC other)
         : base(other.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeBC";
        public override string TypeDescription => "Lemur Boundary Condition";
        public override IGH_GooProxy EmitProxy() => new GH_LeBCProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeBC(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeBoundaryCondition).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)new LeBoundaryCondition(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeBoundaryCondition leBC)
            {
                Value = leBC;
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
            sb.AppendLine($"LeBC:");
            sb.AppendLine($"  TargetGroupName: {Value.TargetGroupName}");
            sb.AppendLine($"  Type: {Value.Type}");
            sb.AppendLine($"  Values: {string.Join(", ", Value.Values)}");
            sb.AppendLine($"  Constraints: {string.Join(", ", Value.Constraints)}");
            return sb.ToString();
        }

        public class GH_LeBCProxy : GH_GooProxy<GH_LeBC>
        {
            public GH_LeBCProxy(GH_LeBC owner) : base(owner) { }
        }
    }
}
