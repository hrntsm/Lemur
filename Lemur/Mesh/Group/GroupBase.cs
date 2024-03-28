namespace Lemur.Mesh.Group
{
    public abstract class GroupBase
    {
        public GroupType Type { get; }
        public string Name { get; }

        public GroupBase(GroupType type, string name)
        {
            Type = type;
            Name = name;
        }

        public abstract string ToMsh();
    }
}
