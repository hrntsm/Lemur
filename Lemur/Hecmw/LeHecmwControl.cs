using System;
using System.IO;
using System.Text;

namespace Lemur.Hecmw
{
    public class LeHecmwControl
    {
        private string _meshFile;
        private string _controlFile;
        private string _resultName;
        private LeMPIType _mpiType;
        private int _numProcess;
        private readonly LePartitionType _partitionType = LePartitionType.NodeBased;
        private readonly LePartitionMethod _partitionMethod = LePartitionMethod.PMETIS;

        public LeHecmwControl()
        {
            string name = "lemur";
            SetValues(name + ".msh", name + ".cnt", name, LeMPIType.Serial, 1);
        }

        public LeHecmwControl(LeHecmwControl other)
        {
            SetValues(other._meshFile, other._controlFile, other._resultName, other._mpiType, other._numProcess);
        }

        private void SetValues(string meshFile, string controlFile, string resultName, LeMPIType mpiType, int numProcess)
        {
            _meshFile = meshFile;
            _controlFile = controlFile;
            _resultName = resultName;
            _mpiType = mpiType;
            _numProcess = numProcess;
        }

        public void SetMPIValues(LeMPIType mpiType, int numProcess)
        {
            _mpiType = mpiType;
            _numProcess = numProcess;
        }

        public string ToDat()
        {
            var sb = new StringBuilder();
            AppendHeader(sb);
            AppendForMesh(sb);
            AppendForSolver(sb);
            AppendForResult(sb);
            sb.AppendLine("!SUBDIR, ON");

            return sb.ToString();
        }

        private static void AppendHeader(StringBuilder sb)
        {
            sb.AppendLine($"#");
            sb.AppendLine($"# Global Control Data File generated by Lemur");
            sb.AppendLine($"# at {DateTime.Now}");
            sb.AppendLine($"#");
        }

        private void AppendForMesh(StringBuilder sb)
        {
            sb.AppendLine($"#");
            sb.AppendLine($"# for mesh");
            sb.AppendLine($"#");
            if (_numProcess > 1)
            {
                sb.AppendLine($"!MESH, NAME=part_in,TYPE=HECMW-ENTIRE");
                sb.AppendLine($" {_meshFile}");
                sb.AppendLine($"!MESH, NAME=part_out, TYPE=HECMW-DIST");
                sb.AppendLine($" {_resultName}.p");
                sb.AppendLine($"!MESH, NAME=fstrMSH, TYPE=HECMW-DIST");
                sb.AppendLine($" {_resultName}.p");
            }
            else
            {
                sb.AppendLine($"!MESH, NAME=fstrMSH,TYPE=HECMW-ENTIRE");
                sb.AppendLine($" {_meshFile}");
            }
            sb.AppendLine($"!MESH, NAME=mesh,TYPE=HECMW-ENTIRE");
            sb.AppendLine($" {_meshFile}");
        }

        private void AppendForSolver(StringBuilder sb)
        {
            sb.AppendLine($"#");
            sb.AppendLine($"# for solver");
            sb.AppendLine($"#");
            sb.AppendLine($"!CONTROL,NAME=fstrCNT");
            sb.AppendLine($" {_controlFile}");
            sb.AppendLine($"!RESTART,NAME=restart_out,IO=OUT");
            sb.AppendLine($" {_resultName}.restart");
        }

        private void AppendForResult(StringBuilder sb)
        {
            sb.AppendLine($"#");
            sb.AppendLine($"# for result");
            sb.AppendLine($"#");
            sb.AppendLine($"!RESULT,NAME=fstrRES,IO=OUT");
            sb.AppendLine($" {_resultName}.res");
            sb.AppendLine($"!RESULT,NAME=result,IO=IN");
            sb.AppendLine($" {_resultName}.res");
            sb.AppendLine($"!RESULT,NAME=vis_out,IO=OUT");
            sb.AppendLine($" {_resultName}.vis");
        }

        private string ToPart()
        {
            var sb = new StringBuilder();
            string type = string.Empty;
            switch (_partitionType)
            {
                case LePartitionType.NodeBased:
                    type = "NODE-BASED";
                    break;
                case LePartitionType.ElementBased:
                    type = "ELEMENT-BASED";
                    break;
            }
            string method = _partitionMethod.ToString();
            sb.AppendLine($"!PARTITION, TYPE={type}, METHOD={method}, DOMAIN={_numProcess}, DEPTH=2");
            return sb.ToString();
        }

        public void Serialize(string dir)
        {
            string path = Path.Combine(dir, "hecmw_ctrl.dat");
            File.WriteAllText(path, ToDat());

            if (_numProcess > 1)
            {
                string path2 = Path.Combine(dir, "hecmw_part_ctrl.dat");
                File.WriteAllText(path2, ToPart());
            }
        }
    }
}
