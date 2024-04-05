using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Mesh;
using Lemur.Mesh.Group;

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

        public GH_LeMesh(GH_LeMesh other)
         : base(other.Value)
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
            string hasResult = Value.Nodes[0].NodalResults.Length > 0 ? "Include Results" : string.Empty;
            var sb = new StringBuilder();
            sb.AppendLine($"LeMesh: {hasResult}");
            sb.AppendLine($"- {Value.Nodes.Count} nodes");
            AppendElement(sb);
            AppendGroup(sb);
            AppendContact(sb);
            AppendMaterial(sb);
            return sb.ToString();
        }

        private void AppendContact(StringBuilder sb)
        {
            sb.AppendLine($"- Contact:");
            if (Value.Contact != null)
            {
                sb.AppendLine($"  - {Value.Contact.Name}: {Value.Contact.Slave} -> {Value.Contact.Master}");
            }
        }

        private void AppendMaterial(StringBuilder sb)
        {
            sb.AppendLine($"- Material:");
            if (Value.Materials != null)
            {
                foreach (LeMaterial material in Value.Materials)
                {
                    string targets = material.TargetEGroups != null
                        ? string.Join(", ", material.TargetEGroups)
                        : "None";
                    sb.AppendLine($"  - {material.Name}: {targets}");
                }
            }
        }

        private void AppendGroup(StringBuilder sb)
        {
            sb.AppendLine($"- Group:");
            if (Value.NodeGroups != null)
            {
                sb.AppendLine($"  - Node Group:");
                foreach (NGroup group in Value.NodeGroups)
                {
                    sb.AppendLine($"    - {group.Name}: {group.Ids.Length} nodes");
                }
            }
            if (Value.ElementGroups != null)
            {
                sb.AppendLine($"  - Element Group:");
                foreach (EGroup group in Value.ElementGroups)
                {
                    sb.AppendLine($"    - {group.Name}: {group.Ids.Length} elements");
                }
            }
            if (Value.SurfaceGroups != null)
            {
                sb.AppendLine($"  - Surface Group:");
                foreach (SGroup group in Value.SurfaceGroups)
                {
                    sb.AppendLine($"    - {group.Name}: {group.Ids.Length} faces");
                }
            }
        }

        private void AppendElement(StringBuilder sb)
        {
            sb.AppendLine($"- Element:");
            foreach (LeElementList elementList in Value.Elements)
            {
                sb.AppendLine($"  - {elementList.Count} {elementList.ElementType} elements");
            }
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
