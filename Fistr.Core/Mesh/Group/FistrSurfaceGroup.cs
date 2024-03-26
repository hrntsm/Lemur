using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Fistr.Core.Mesh.Group
{
    public class FistrSurfaceGroup : FistrGroupBase
    {
        public Dictionary<int, int> Ids { get; }

        public FistrSurfaceGroup(string name, Dictionary<int, int> ids)
            : base(FistrGroupType.Surface, name)
        {
            Ids = ids;
        }

        public override string ToMsh()
        {
            var sb = new StringBuilder();
            sb.Append($"!SGROUP, SGRP={Name}");
            int count = 0;
            foreach (KeyValuePair<int, int> id in Ids)
            {
                sb.Append(id.Key.ToString(CultureInfo.InvariantCulture).PadLeft(9, ' '));
                sb.Append(',');
                sb.Append(id.Value.ToString(CultureInfo.InvariantCulture).PadLeft(4, ' '));
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

            return sb.ToString();
        }
    }
}
