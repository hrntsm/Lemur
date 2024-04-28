using System;
using System.Text;

namespace Lemur.Material
{
    [Serializable]
    public class LeMaterialElastic : LeMaterialBase
    {
        public LeMaterialElastic(LeMaterialBase other)
          : base(other)
        {
        }

        public LeMaterialElastic(string name, double density, double youngsModulus, double poissonRatio)
          : base(name, density, youngsModulus, poissonRatio)
        {
        }

        public override LeMaterialBase Clone()
        {
            return new LeMaterialElastic(this);
        }

        public override string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!MATERIAL, NAME={Name}");
            sb.AppendLine($"!ELASTIC, TYPE=ISOTROPIC");
            sb.AppendLine($" {YoungsModulus}, {PoissonRatio}");
            sb.AppendLine($"!DENSITY");
            sb.AppendLine($" {Density}");
            return sb.ToString();
        }
    }
}
