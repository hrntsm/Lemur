using System;
using System.Globalization;
using System.Text;

namespace Lemur.Mesh.Group
{
    public class NGroup : GroupBase
    {
        public int[] Ids { get; }

        public NGroup(string name, int[] ids)
         : base(GroupType.Node, name)
        {
            Ids = ids;
        }

        public override string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!NGROUP, NGRP={Name}");
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
