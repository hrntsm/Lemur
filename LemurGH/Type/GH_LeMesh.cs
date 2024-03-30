using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Mesh;

namespace LemurGH.Type
{
    public class GH_LeMesh : GH_Goo<LeMesh>
    {
        public GH_LeMesh()
        {
        }

        public GH_LeMesh(LeMesh leMesh)
         : base(leMesh)
        {
        }

        public GH_LeMesh(GH_LeMesh leMesh)
         : base(leMesh.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeMesh";
        public override string TypeDescription => "Lemur Mesh";
        public override IGH_GooProxy EmitProxy() => new GH_LeMeshProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeMesh(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeMesh).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)new LeMesh(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeMesh leMesh)
            {
                Value = leMesh;
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
            sb.AppendLine($"LeMesh:");
            sb.AppendLine($"- {Value.Nodes.Count} nodes");
            sb.AppendLine($"- Element:");
            foreach (LeElementList elementList in Value.Elements)
            {
                sb.AppendLine($"  - {elementList.Count} {elementList.ElementType} elements");
            }
            return sb.ToString();
        }

        public class GH_LeMeshProxy : GH_GooProxy<GH_LeMesh>
        {
            public GH_LeMeshProxy(GH_LeMesh owner)
             : base(owner)
            {
            }
        }
    }
}
