using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Mesh;

namespace LemurGH.Type
{
    public class GH_LeContactMesh : GH_Goo<LeContactMesh>
    {
        public GH_LeContactMesh()
        {
        }

        public GH_LeContactMesh(LeContactMesh leContactMesh)
         : base(leContactMesh)
        {
        }

        public GH_LeContactMesh(GH_LeContactMesh other)
         : base(other.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeContactMesh";
        public override string TypeDescription => "Lemur Contact Mesh";
        public override IGH_GooProxy EmitProxy() => new GH_LeContactMeshProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeContactMesh(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeContactMesh).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)new LeContactMesh(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeContactMesh leContactMesh)
            {
                Value = leContactMesh;
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
            sb.AppendLine($"LeContactMesh:");
            sb.AppendLine($" {Value.Name}: {Value.Slave} -> {Value.Master}");
            return sb.ToString();
        }

        public class GH_LeContactMeshProxy : GH_GooProxy<GH_LeContactMesh>
        {
            public GH_LeContactMeshProxy(GH_LeContactMesh owner) : base(owner) { }
        }
    }
}
