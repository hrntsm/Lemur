using System;
using System.Globalization;
using System.Text;

namespace Fistr.Core.Mesh.Group
{
    public class FistrGroup : FistrGroupBase
    {
        public int[] Ids { get; }

        public FistrGroup(FistrGroupType type, string name, int[] ids)
         : base(type, name)
        {
            Ids = ids;
        }

        public override string ToMsh()
        {
            var sb = new StringBuilder();
            switch (Type)
            {
                case FistrGroupType.Node:
                    sb.Append($"!NGROUP, NGRP={Name}");
                    break;
                case FistrGroupType.Element:
                    sb.Append($"!EGROUP, EGRP={Name}");
                    break;
                default:
                    throw new ArgumentException("Invalid group type");
            }
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

            return sb.ToString();
        }
    }
}
