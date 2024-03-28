using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lemur.Mesh.Group
{
    public class SGroup : GroupBase
    {
        public Dictionary<int, int> Ids { get; }

        public SGroup(string name, Dictionary<int, int> ids)
            : base(GroupType.Surface, name)
        {
            Ids = ids;
        }

        public override string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!SGROUP, SGRP={Name}");
            int count = 0;
            foreach (KeyValuePair<int, int> id in Ids)
            {
                sb.Append(id.Key.ToString(CultureInfo.InvariantCulture).PadLeft(9, ' '));
                sb.Append(',');
                sb.Append(id.Value.ToString(CultureInfo.InvariantCulture).PadLeft(3, ' '));
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
