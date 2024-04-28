using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Material;

namespace LemurGH.Type
{
    public class GH_LeMaterial : GH_Goo<LeMaterialBase>
    {
        public GH_LeMaterial()
        {
        }

        public GH_LeMaterial(LeMaterialBase leMaterial)
         : base(leMaterial)
        {
        }

        public GH_LeMaterial(GH_LeMaterial other)
         : base(other.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeMaterial";
        public override string TypeDescription => "Lemur Material";
        public override IGH_GooProxy EmitProxy() => new GH_LeMaterialProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeMaterial(Value);
        public override bool CastTo<T>(ref T target)
        {
            if (typeof(LeMaterialElastic).IsAssignableFrom(typeof(T)))
            {
                target = (T)(object)new LeMaterialElastic(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeMaterialElastic leMaterial)
            {
                Value = leMaterial;
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
            sb.AppendLine($"LeMaterial:");
            sb.AppendLine($"Name: {Value.Name}");
            sb.AppendLine($"Density: {Value.Density}");
            sb.AppendLine($"YoungsModulus: {Value.YoungsModulus}");
            sb.AppendLine($"PoissonRatio: {Value.PoissonRatio}");
            return sb.ToString();
        }

        public class GH_LeMaterialProxy : GH_GooProxy<GH_LeMaterial>
        {
            public GH_LeMaterialProxy(GH_LeMaterial owner) : base(owner) { }
        }
    }
}
