using System.Text;

using Grasshopper.Kernel.Types;

using Lemur.Control.Solver;


namespace LemurGH.Type
{
    public class GH_LeSolver : GH_Goo<LeSolver>
    {
        public GH_LeSolver()
        {
        }

        public GH_LeSolver(LeSolver leSolver)
         : base(leSolver)
        {
        }

        public GH_LeSolver(GH_LeSolver other)
         : base(other.Value)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "LeSolver";
        public override string TypeDescription => "Lemur Solver";
        public override IGH_GooProxy EmitProxy() => new GH_LeSolverProxy(this);
        public override IGH_Goo Duplicate() => new GH_LeSolver(Value);
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(LeSolver).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)new LeSolver(Value);
                return true;
            }
            else
            {
                return base.CastTo(ref target);
            }
        }

        public override bool CastFrom(object source)
        {
            if (source is LeSolver leSolver)
            {
                Value = leSolver;
                return true;
            }
            else
            {
                return base.CastFrom(source);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"LeSolver:");
            sb.AppendLine($" {Value.Method}, {Value.Precondition}, {Value.MaxIter}, {Value.Residual}");
            return sb.ToString();
        }

        public class GH_LeSolverProxy : GH_GooProxy<GH_LeSolver>
        {
            public GH_LeSolverProxy(GH_LeSolver owner) : base(owner) { }
        }
    }
}
