using System;
using System.Globalization;
using System.Text;

namespace Lemur.Mesh.Group
{
    public class SGroup : GroupBase
    {
        public (int, int)[] Ids { get; }

        public SGroup(string name, (int, int)[] ids)
            : base(GroupType.Surface, name)
        {
            Ids = ids;
        }

        public override string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!SGROUP, SGRP={Name}");
            int count = 0;
            foreach ((int, int) id in Ids)
            {
                sb.Append(id.Item1.ToString(CultureInfo.InvariantCulture).PadLeft(9, ' '));
                sb.Append(',');
                sb.Append(id.Item2.ToString(CultureInfo.InvariantCulture).PadLeft(3, ' '));
                sb.Append(',');
                if (count % 5 == 4)
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
