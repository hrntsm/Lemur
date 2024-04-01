using System.Text;

namespace Lemur.Mesh
{
    public class LeContactMesh
    {
        public string Name { get; }
        public string Slave { get; }
        public string Master { get; }

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
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!CONTACT PAIR, NAME={Name}");
            sb.AppendLine($"{Slave}, {Master}");
            return sb.ToString();
        }
    }
}
