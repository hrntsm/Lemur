using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Material
{
    [Serializable]
    public class LeMaterialPlasticMises : LeMaterialBase
    {
        private readonly List<double> _plasticStress;
        private readonly List<double> _plasticStrain;

        public LeMaterialPlasticMises(LeMaterialPlasticMises other)
          : base(other)
        {
            _plasticStress = new List<double>(other._plasticStress);
            _plasticStrain = new List<double>(other._plasticStrain);
        }

        public LeMaterialPlasticMises(string name, double density, double youngsModulus, double poissonRatio, List<double> plasticStress, List<double> plasticStrain)
          : base(name, density, youngsModulus, poissonRatio)
        {
            if (plasticStress.Count != plasticStrain.Count)
            {
                throw new ArgumentException("Plastic stress and strain lists must have the same length.");
            }
            _plasticStress = plasticStress;
            _plasticStrain = plasticStrain;
        }

        public override LeMaterialBase Clone()
        {
            return new LeMaterialPlasticMises(this);
        }

        public override string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!MATERIAL, NAME={Name}");
            sb.AppendLine($"!ELASTIC, TYPE=ISOTROPIC");
            sb.AppendLine($" {YoungsModulus}, {PoissonRatio}");
            sb.AppendLine($"!PLASTIC, YIELD=MISES, HARDEN=MULTILINEAR");
            for (int i = 0; i < _plasticStress.Count; i++)
            {
                sb.AppendLine($" {_plasticStress[i]}, {_plasticStrain[i]}");
            }
            sb.AppendLine($"!DENSITY");
            sb.AppendLine($" {Density}");
            return sb.ToString();
        }
    }
}
