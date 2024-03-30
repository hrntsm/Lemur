using System.Text;

namespace Lemur.Control
{
    public class LeSection
    {
        public LeForm341Type Form341Type { get; } = LeForm341Type.FI;
        public LeForm361Type Form361Type { get; } = LeForm361Type.FI;

        public LeSection()
        {
        }

        public LeSection(LeSection other)
        {
            Form341Type = other.Form341Type;
            Form361Type = other.Form361Type;
        }

        public string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!SECTION");
            if (Form341Type != LeForm341Type.Default)
            {
                sb.AppendLine($" FORM341={Form341Type}");
            }
            if (Form361Type != LeForm361Type.Default)
            {
                sb.AppendLine($" FORM361={Form361Type}");
            }
            return sb.ToString();
        }
    }
}
