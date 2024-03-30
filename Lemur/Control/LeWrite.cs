using System;
using System.Linq;
using System.Text;

namespace Lemur.Control
{
    public class LeWrite
    {
        public LeWriteType LeWriteType { get; }
        public int Frequency { get; } = 1;
        public LeOutputType[] LeOutputTypes { get; private set; }

        public LeWrite(LeWriteType leWriteType)
        {
            LeWriteType = leWriteType;
            SetDefaultLeOutputType();
        }

        public LeWrite(LeWriteType leWriteType, int frequency)
        {
            LeWriteType = leWriteType;
            Frequency = frequency;
            SetDefaultLeOutputType();
        }

        public LeWrite(LeWriteType leWriteType, int frequency, LeOutputType[] leOutputTypes)
        {
            LeWriteType = leWriteType;
            Frequency = frequency;
            CheckLeOutputTypes(leOutputTypes);
            LeOutputTypes = leOutputTypes;
        }

        private void CheckLeOutputTypes(LeOutputType[] leOutputTypes)
        {
            switch (LeWriteType)
            {
                case LeWriteType.VISUAL:
                    if (leOutputTypes.Contains(LeOutputType.ESTRESS) ||
                        leOutputTypes.Contains(LeOutputType.EMISES) ||
                        leOutputTypes.Contains(LeOutputType.ESTRAIN) ||
                        leOutputTypes.Contains(LeOutputType.ISTRAIN) ||
                        leOutputTypes.Contains(LeOutputType.ISTRESS) ||
                        leOutputTypes.Contains(LeOutputType.PL_ISTRAIN) ||
                        leOutputTypes.Contains(LeOutputType.PRINC_ESTRESS) ||
                        leOutputTypes.Contains(LeOutputType.PRINCV_ESTRESS) ||
                        leOutputTypes.Contains(LeOutputType.PRINC_ESTRAIN) ||
                        leOutputTypes.Contains(LeOutputType.PRINCV_ESTRAIN))
                    {
                        throw new ArgumentException("LeOutputTypes must not contain elements or integration point output for LeWriteType.VISUAL");
                    }
                    break;
                case LeWriteType.RESULT:
                    break;
                case LeWriteType.LOG:
                    if (leOutputTypes.Length > 0)
                    {
                        throw new ArgumentException("LeOutputTypes must be empty for LeWriteType.LOG");
                    }
                    break;
            }
        }

        private void SetDefaultLeOutputType()
        {
            switch (LeWriteType)
            {
                case LeWriteType.VISUAL:
                    LeOutputTypes = new LeOutputType[]
                     {
                        LeOutputType.DISP,
                        LeOutputType.NSTRESS, LeOutputType.NMISES
                     };
                    break;
                case LeWriteType.RESULT:
                    LeOutputTypes = new LeOutputType[]
                    {
                        LeOutputType.DISP,
                        LeOutputType.NSTRESS, LeOutputType.NMISES,
                        LeOutputType.ESTRESS, LeOutputType.EMISES
                    };
                    break;
                case LeWriteType.LOG:
                    break;
            }
        }

        public string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!WRITE, {LeWriteType}, FREQUENCY={Frequency}");

            if (LeWriteType == LeWriteType.LOG)
            {
                return sb.ToString();
            }

            if (LeWriteType == LeWriteType.VISUAL)
            {
                sb.AppendLine($"!OUTPUT_VIS");
            }
            else if (LeWriteType == LeWriteType.RESULT)
            {
                sb.AppendLine($"!OUTPUT_RES");
            }

            foreach (LeOutputType leOutputType in LeOutputTypes)
            {
                sb.AppendLine($" {leOutputType}, ON");
            }

            return sb.ToString();
        }
    }
}
