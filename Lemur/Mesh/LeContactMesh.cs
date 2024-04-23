using System;
using System.Linq;
using System.Text;

using Lemur.Mesh.Group;

namespace Lemur.Mesh
{
    [Serializable]
    public class LeContactMesh
    {
        public string Name { get; }
        public string Slave { get; }
        public string Master { get; }
        public LeContactPairType Type { get; private set; }

        public LeContactMesh(string name, string slave, string master)
        {
            Name = name;
            Slave = slave;
            Master = master;
        }

        public LeContactMesh(LeContactMesh other)
        {
            Name = other.Name;
            Slave = other.Slave;
            Master = other.Master;
            Type = other.Type;
        }

        public void SetTypeUsingGroups(LeGroupBase[] groups)
        {
            LeGroupBase slaveGroup = groups.FirstOrDefault(g => g.Name == Slave);
            if (slaveGroup == null)
            {
                throw new ArgumentException($"Group:{Slave} does not exist in this mesh group list.");
            }

            switch (slaveGroup.Type)
            {
                case LeGroupType.Node:
                    Type = LeContactPairType.NODE_SURF;
                    break;
                case LeGroupType.Surface:
                    Type = LeContactPairType.SURF_SURF;
                    break;
                default:
                    throw new ArgumentException($"Group:{Slave} is not a valid type for contact pair.");
            }
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            string typeString = Type == LeContactPairType.NODE_SURF ? "NODE-SURF" : "SURF-SURF";
            sb.AppendLine($"!CONTACT PAIR, NAME={Name}, TYPE={typeString}");
            sb.AppendLine($"{Slave}, {Master}");
            return sb.ToString();
        }
    }
}
