using System.Collections.Generic;
using System.IO;
using System.Text;

using Lemur.Control.BoundaryCondition;
using Lemur.Control.Output;
using Lemur.Control.Section;

namespace Lemur.Control
{
    public class LeControl
    {
        public int Version { get; } = 5;
        public LeSolutionType SolutionType { get; }
        public LeWrite[] LeWrites { get; }
        public LeSection LeSection { get; }
        public LeBoundaryCondition[] LeBoundaryConditions => _leBC.ToArray();

        private readonly List<LeBoundaryCondition> _leBC;

        public LeControl()
        {
            SolutionType = LeSolutionType.STATIC;
            LeWrites = new LeWrite[]
            {
                new LeWrite(LeWriteType.VISUAL),
                new LeWrite(LeWriteType.RESULT),
                new LeWrite(LeWriteType.LOG),
            };
            LeSection = new LeSection();
            _leBC = new List<LeBoundaryCondition>();
        }

        public LeControl(LeControl other)
        {
            Version = other.Version;
            SolutionType = other.SolutionType;
            LeWrites = new LeWrite[other.LeWrites.Length];
            for (int i = 0; i < other.LeWrites.Length; i++)
            {
                LeWrites[i] = new LeWrite(other.LeWrites[i]);
            }
            LeSection = new LeSection(other.LeSection);
            _leBC = new List<LeBoundaryCondition>(other.LeBoundaryConditions);
        }

        public void AddBC(LeBoundaryCondition leBC)
        {
            _leBC.Add(leBC);
        }

        public void ClearBC()
        {
            _leBC.Clear();
        }

        public string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!Version");
            sb.AppendLine($" {Version}");
            sb.AppendLine();
            sb.AppendLine($"!SOLUTION, TYPE={SolutionType}");
            sb.AppendLine();
            foreach (LeWrite leWrite in LeWrites)
            {
                sb.AppendLine(leWrite.ToCnt());
            }

            // 多分フォーマットがあってなくてFistrでエラーになる
            // sb.AppendLine(LeSection.ToCnt());

            foreach (LeBoundaryCondition leBC in _leBC)
            {
                sb.AppendLine(leBC.ToCnt());
            }

            sb.AppendLine(VtkOutput());
            sb.AppendLine($"!END");
            return sb.ToString();
        }

        private static string VtkOutput()
        {
            var sb = new StringBuilder();
            sb.AppendLine("!VISUAL,method=PSR");
            sb.AppendLine("!surface_num=1");
            sb.AppendLine("!surface 1");
            sb.AppendLine("!output_type=VTK");
            return sb.ToString();
        }

        public void Serialize(string dir)
        {
            Serialize(dir, "lemur.cnt");
        }

        public void Serialize(string dir, string name)
        {
            File.WriteAllText(Path.Combine(dir, name), ToCnt());
        }
    }
}
