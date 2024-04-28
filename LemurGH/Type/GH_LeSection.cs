using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Section;

namespace LemurGH.Type
{
    public class GH_LeSection : GH_Goo<LeSection>
    {
        public GH_LeSection()
        {
        }

        public GH_LeSection(LeSection leSection)
         : base(leSection)
        {
        }

        public GH_LeSection(GH_LeSection other)
         : base(other.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeSection";
        public override string TypeDescription => "Lemur Section";
        public override IGH_GooProxy EmitProxy() => new GH_LeSectionProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeSection(Value);
        public override bool CastTo<T>(ref T target)
        {
            if (typeof(LeSection).IsAssignableFrom(typeof(T)))
            {
                target = (T)(object)new LeSection(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeSection leSection)
            {
                Value = leSection;
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
            sb.AppendLine($"LeSection:");
            sb.AppendLine($"Id: {Value.Id}");
            sb.AppendLine($"Type: {Value.SectionType}");
            sb.AppendLine($"Material: {Value.Material.Name}");
            return sb.ToString();
        }

        public class GH_LeSectionProxy : GH_GooProxy<GH_LeSection>
        {
            public GH_LeSectionProxy(GH_LeSection owner) : base(owner) { }
        }
    }
}
