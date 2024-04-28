using System;
using System.Text;

using Lemur.Material;

namespace Lemur.Section
{
    [Serializable]
    public class LeSection
    {
        public int Id { get; set; } = -1;
        public string[] TargetEGroups { get; }
        public LeSectionType SectionType { get; } = LeSectionType.SOLID;
        public LeForm341Type Form341Type { get; } = LeForm341Type.SELECTIVE_ESNS;
        public LeForm361Type Form361Type { get; } = LeForm361Type.Default;
        public LeMaterialBase Material { get; }
        private LeBeamSection _beamSection;
        private LeShellSection _shellSection;

        public LeSection(int id, string[] targets, LeSectionType sectionType, LeMaterialBase material)
        {
            Id = id;
            TargetEGroups = targets;
            SectionType = sectionType;
            Material = material;
        }

        public LeSection(LeSection other)
        {
            Id = other.Id;
            TargetEGroups = other.TargetEGroups;
            SectionType = other.SectionType;
            Form341Type = other.Form341Type;
            Form361Type = other.Form361Type;
            Material = other.Material.Clone();
            _beamSection = other._beamSection;
            _shellSection = other._shellSection;
        }

        public void SetShellParameter(double thickness, int numIntegrationPoints)
        {
            if (SectionType != LeSectionType.SHELL)
            {
                throw new InvalidOperationException("Section type must be SHELL.");
            }
            _shellSection = new LeShellSection(thickness, numIntegrationPoints);
        }

        public void SetBeamParameter(double[] coordinates, double area, double iyy, double izz, double jx)
        {
            if (SectionType == LeSectionType.SHELL || SectionType == LeSectionType.INTERFACE)
            {
                throw new InvalidOperationException("Section type must be SOLID or BEAM.");
            }
            _beamSection = new LeBeamSection(coordinates, area, iyy, izz, jx);
        }

        public string ToCnt()
        {
            var sb = new StringBuilder();
            sb.Append($"!SECTION, SECNUM={Id}, ");
            if (Form341Type != LeForm341Type.Default)
            {
                sb.Append($"FORM341={Form341Type}");
            }
            if (Form361Type != LeForm361Type.Default)
            {
                sb.Append($" FORM361={Form361Type}");
            }
            sb.AppendLine(Environment.NewLine);
            sb.Append(Material.ToCnt());
            return sb.ToString();
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!MATERIAL, NAME={Material.Name}");
            sb.AppendLine($"!ITEM=1, SUBITEM=2");
            sb.AppendLine($" {Material.YoungsModulus}, {Material.PoissonRatio}");
            foreach (string target in TargetEGroups)
            {
                sb.AppendLine($"!SECTION, TYPE={SectionType}, EGRP={target}, MATERIAL={Material.Name}");
            }

            switch (SectionType)
            {
                case LeSectionType.SOLID when _beamSection != null:
                    return ToMshTruss(sb);
                case LeSectionType.SOLID:
                    return sb.ToString();
                case LeSectionType.SHELL:
                    return ToMshShell(sb);
                case LeSectionType.BEAM:
                    return ToMshBeam(sb);
                case LeSectionType.INTERFACE:
                    return ToMshInterface(sb);
                default:
                    throw new NotImplementedException();
            }
        }

        private string ToMshTruss(StringBuilder sb)
        {
            if (_beamSection == null)
            {
                throw new InvalidOperationException("Beam section must be set.");
            }
            sb.AppendLine($" {_beamSection.Area}");
            return sb.ToString();
        }

        private string ToMshShell(StringBuilder sb)
        {
            if (_shellSection == null)
            {
                throw new InvalidOperationException("Shell section must be set.");
            }
            sb.AppendLine($" {_shellSection.Thickness}, {_shellSection.NumIntegrationPoints}");
            return sb.ToString();
        }

        private string ToMshBeam(StringBuilder sb)
        {
            if (_beamSection == null)
            {
                throw new InvalidOperationException("Beam section must be set.");
            }
            LeBeamSection b = _beamSection;
            sb.AppendLine($" {b.Coordinates[0]}, {b.Coordinates[1]}, {b.Coordinates[2]}, {b.Coordinates[3]}, {b.Area}, {b.Iyy}, {b.Izz}, {b.Jx}");
            return sb.ToString();
        }

        private string ToMshInterface(StringBuilder sb)
        {
            throw new NotImplementedException();
        }
    }
}
