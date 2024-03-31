namespace Lemur.Mesh.Group
{
    public abstract class LeGroupBase
    {
        public LeGroupType Type { get; }
        public string Name { get; }

        public LeGroupBase(LeGroupType type, string name)
        {
            Type = type;
            Name = name;
        }

        public abstract string ToMsh();
    }
}
