using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Control.Contact;


namespace LemurGH.Type
{
    public class GH_LeContactControl : GH_Goo<LeContactControl>
    {
        public GH_LeContactControl()
        {
        }

        public GH_LeContactControl(LeContactControl leContactControl)
         : base(leContactControl)
        {
        }

        public GH_LeContactControl(GH_LeContactControl other)
         : base(other.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeContactControl";
        public override string TypeDescription => "Lemur Contact Control";
        public override IGH_GooProxy EmitProxy() => new GH_LeContactControlProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeContactControl(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeContactControl).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)new LeContactControl(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeContactControl leContactControl)
            {
                Value = leContactControl;
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
            sb.AppendLine($"LeContactControl:");
            sb.AppendLine($"  {Value.Algorithm}, {Value.Interaction}, Pair:{Value.TargetContactPair}");
            return sb.ToString();
        }

        public class GH_LeContactControlProxy : GH_GooProxy<GH_LeContactControl>
        {
            public GH_LeContactControlProxy(GH_LeContactControl owner) : base(owner) { }
        }
    }
}
