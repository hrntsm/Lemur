using System.Text;

namespace Lemur.Mesh
{
    public class LeMaterial
    {
        public string Name { get; }
        public double Density { get; }
        public double YoungsModulus { get; }
        public double PoissonRatio { get; }
        public string[] TargetEGroups { get; }

        public LeMaterial(string name, double density, double youngsModulus, double poissonRatio, string[] targetEGroups)
        {
            Name = name;
            Density = density;
            YoungsModulus = youngsModulus;
            PoissonRatio = poissonRatio;
            TargetEGroups = targetEGroups;
        }

        public LeMaterial(LeMaterial other)
        {
            Name = other.Name;
            Density = other.Density;
            YoungsModulus = other.YoungsModulus;
            PoissonRatio = other.PoissonRatio;
            TargetEGroups = other.TargetEGroups;
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!MATERIAL, NAME={Name}, ITEM=2");
            sb.AppendLine("!ITEM=1, SUBITEM=2");
            sb.AppendLine($"{YoungsModulus}, {PoissonRatio}");
            sb.AppendLine("!ITEM=2, SUBITEM=1");
            sb.AppendLine($"{Density}");
            foreach (string targetEGroup in TargetEGroups)
            {
                sb.AppendLine($"!SECTION, TYPE=SOLID, EGRP={targetEGroup}, MATERIAL={Name}");
            }
            return sb.ToString();
        }
    }
}
