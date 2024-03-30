using Lemur.Control;
using Lemur.Mesh;

namespace Lemur
{
    public class LeAssemble
    {
        public LeMesh LeMesh { get; }
        public LeControl LeControl { get; }
        public LeHecmwControl LeHecmwControl { get; }

        public LeAssemble(LeMesh leMesh, LeControl leControl, LeHecmwControl leHecmwControl)
        {
            LeMesh = leMesh;
            LeControl = leControl;
            LeHecmwControl = leHecmwControl;
        }

        public LeAssemble(LeAssemble other)
        {
            LeMesh = new LeMesh(other.LeMesh);
            LeControl = new LeControl(other.LeControl);
            LeHecmwControl = new LeHecmwControl(other.LeHecmwControl);
        }

        public void Serialize(string dir)
        {
            LeMesh?.Serialize(dir);
            LeControl?.Serialize(dir);
            LeHecmwControl?.Serialize(dir);
        }
    }
}
