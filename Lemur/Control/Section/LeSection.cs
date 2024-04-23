using System;
using System.Text;

namespace Lemur.Control.Section
{
    [Serializable]
    public class LeSection
    {
        public LeForm341Type Form341Type { get; } = LeForm341Type.SELECTIVE_ESNS;
        public LeForm361Type Form361Type { get; } = LeForm361Type.Default;

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
            sb.Append($"!SECTION, SECNUM=1, ");
            if (Form341Type != LeForm341Type.Default)
            {
                sb.Append($"FORM341={Form341Type}");
            }
            if (Form361Type != LeForm361Type.Default)
            {
                sb.Append($" FORM361={Form361Type}");
            }
            sb.AppendLine(Environment.NewLine);
            return sb.ToString();
        }
    }
}
