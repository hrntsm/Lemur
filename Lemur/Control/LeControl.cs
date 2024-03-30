using System;
using System.IO;
using System.Text;

namespace Lemur.Control
{
    public class LeControl
    {
        public int Version { get; } = 5;
        public LeSolutionType SolutionType { get; }
        public LeWrite[] LeWrites { get; }
        public LeSection LeSection { get; }

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
