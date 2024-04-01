using System.Text;

namespace Lemur.Control.Solver
{
    public class LeSolver
    {
        public LeSolverMethod Method { get; }
        public LePrecondition Precondition { get; }
        public int MaxIter { get; }
        public double Residual { get; }

        public LeSolver(LeSolverMethod method, LePrecondition precondition, int maxIter, double residual)
        {
            Method = method;
            Precondition = precondition;
            MaxIter = maxIter;
            Residual = residual;
        }

        public LeSolver(LeSolver other)
        {
            Method = other.Method;
            Precondition = other.Precondition;
            MaxIter = other.MaxIter;
            Residual = other.Residual;
        }

        public string ToCnt()
        {
            if ((int)Method >= 10) // Direct solvers
            {
                return $"!SOLVER,METHOD={Method}";
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine($"!SOLVER,METHOD={Method},PRECOND={(int)Precondition},ITERLOG=YES,TIMELOG=YES");
                sb.AppendLine($" {MaxIter}, 1");
                sb.AppendLine($" {Residual}, 1.0, 1.0");
                return sb.ToString();
            }
        }
    }
}
