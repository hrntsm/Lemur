using System;
using System.Globalization;
using System.Text;

namespace Lemur.Control.BoundaryCondition
{
    [Serializable]
    public class LeBoundaryCondition
    {
        public int Id { get; }
        public string TargetGroupName { get; }
        public LeBCType Type { get; }
        public double[] Values { get; }
        public bool[] Constraints { get; }

        public LeBoundaryCondition(string targetGroupName, LeBCType type, double[] value, bool[] constraints)
        {
            Id = 1;
            TargetGroupName = targetGroupName;
            Type = type;
            Values = value;
            Constraints = constraints;
        }

        public LeBoundaryCondition(LeBoundaryCondition other)
        {
            Id = other.Id;
            TargetGroupName = other.TargetGroupName;
            Type = other.Type;
            Values = new double[other.Values.Length];
            other.Values.CopyTo(Values, 0);
            Constraints = new bool[other.Constraints.Length];
            other.Constraints.CopyTo(Constraints, 0);
        }

        public string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!{Type}, GRPID={Id}");
            switch (Type)
            {
                case LeBCType.BOUNDARY:
                    for (int i = 0; i < Values.Length; i++)
                    {
                        if (Constraints[i])
                        {
                            string value = Values[i].ToString("0.0000000000E+00", CultureInfo.InvariantCulture).PadLeft(18, ' ');
                            sb.AppendLine($" {TargetGroupName}, {i + 1}, {i + 1},{value}");
                        }
                    }
                    break;
                case LeBCType.SPRING:
                case LeBCType.CLOAD:
                case LeBCType.DLOAD:
                    for (int i = 0; i < Values.Length; i++)
                    {
                        if (Constraints[i])
                        {
                            string value = Values[i].ToString("0.0000000000E+00", CultureInfo.InvariantCulture).PadLeft(18, ' ');
                            sb.AppendLine($" {TargetGroupName}, {i + 1},{value}");
                        }
                    }
                    break;
            }
            return sb.ToString();
        }
    }
}
