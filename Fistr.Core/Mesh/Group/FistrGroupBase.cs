namespace Fistr.Core.Mesh.Group
{
    public abstract class FistrGroupBase
    {
        public FistrGroupType Type { get; }
        public string Name { get; }

        public FistrGroupBase(FistrGroupType type, string name)
        {
            Type = type;
            Name = name;
        }

        public abstract string ToMsh();
    }
}
