using System;
using System.Text;

namespace Lemur.Control.Contact
{
    [Serializable]
    public class LeContactControl
    {
        public LeContactAlgorithm Algorithm { get; }
        public LeContactInteraction Interaction { get; }
        public string TargetContactPair { get; }
        public double FrictionCoef { get; }
        public double FrictionPenalty { get; } = 1.0e+5;

        public LeContactControl(LeContactAlgorithm algorithm, LeContactInteraction interaction, string targetContactPair)
        {
            Algorithm = algorithm;
            Interaction = interaction;
            TargetContactPair = targetContactPair;
        }

        public LeContactControl(LeContactControl other)
        {
            Algorithm = other.Algorithm;
            Interaction = other.Interaction;
            TargetContactPair = other.TargetContactPair;
        }

        public string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!CONTACT_ALGO, TYPE={Algorithm}");
            sb.AppendLine($"!CONTACT, GRPID=1, INTERACTION={Interaction}");
            sb.AppendLine($" {TargetContactPair}, {FrictionCoef}, {FrictionPenalty}");
            return sb.ToString();
        }
    }
}
