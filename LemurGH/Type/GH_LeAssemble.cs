using System.Text;

using Grasshopper.Kernel.Types;

using Lemur;

namespace LemurGH.Type
{
    public class GH_LeAssemble : GH_Goo<LeAssemble>
    {
        public GH_LeAssemble()
        {
        }

        public GH_LeAssemble(LeAssemble leAssemble)
         : base(leAssemble)
        {
        }

        public GH_LeAssemble(GH_LeAssemble leAssemble)
         : base(leAssemble.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeAssemble";
        public override string TypeDescription => "Lemur Assemble";
        public override IGH_GooProxy EmitProxy() => new GH_LeAssembleProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeAssemble(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeAssemble).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)new LeAssemble(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeAssemble leAssemble)
            {
                Value = leAssemble;
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
            sb.AppendLine(new GH_LeMesh(Value.LeMesh).ToString());
            sb.AppendLine(new GH_LeControl(Value.LeControl).ToString());
            return sb.ToString();
        }

        public class GH_LeAssembleProxy : GH_GooProxy<GH_LeAssemble>
        {
            public GH_LeAssembleProxy(GH_LeAssemble owner)
             : base(owner)
            {
            }
        }
    }
}
