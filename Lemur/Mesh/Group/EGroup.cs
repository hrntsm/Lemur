using System;
using System.Globalization;
using System.Text;

namespace Lemur.Mesh.Group
{
    [Serializable]
    public class EGroup : LeGroupBase
    {
        public int[] Ids { get; }

        public EGroup(string name, int[] ids)
         : base(LeGroupType.Element, name)
        {
            Ids = ids;
        }

        public override string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!EGROUP, EGRP={Name}");
            int count = 0;
            foreach (int id in Ids)
            {
                sb.Append(id.ToString(CultureInfo.InvariantCulture).PadLeft(9, ' '));
                sb.Append(',');
                if (count % 10 == 9)
                {
                    sb.Append(Environment.NewLine);
                    count = 0;
                }
                else
                {
                    count++;
                }
            }

            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
