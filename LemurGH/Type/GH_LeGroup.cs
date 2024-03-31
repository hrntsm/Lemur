using Grasshopper.Kernel.Types;

using Lemur.Mesh.Group;

namespace LemurGH.Type
{
    public class GH_LeGroup : GH_Goo<LeGroupBase>
    {
        public GH_LeGroup()
        {
        }

        public GH_LeGroup(LeGroupBase leGroup)
         : base(leGroup)
        {
        }

        public GH_LeGroup(GH_LeGroup leGroup)
         : base(leGroup.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeGroup";
        public override string TypeDescription => "Lemur Group";
        public override IGH_GooProxy EmitProxy() => new GH_LeGroupProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeGroup(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeGroupBase).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)Value;
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeGroupBase leGroup)
            {
                Value = leGroup;
                return true;
            }
            else
            {
                return base.CastFrom(source);
            }
        }
        public override string ToString() => Value.ToString();

        public class GH_LeGroupProxy : GH_GooProxy<GH_LeGroup>
        {
            public GH_LeGroupProxy(GH_LeGroup owner)
             : base(owner)
            {
            }
        }
    }
}
