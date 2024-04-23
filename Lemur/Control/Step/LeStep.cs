using System;
using System.Text;

namespace Lemur.Control.Step
{
    [Serializable]
    public class LeStep
    {
        public int[] BoundaryIds { get; private set; }
        public int[] LoadIds { get; private set; }
        public int[] ContactIds { get; private set; }
        public int SubSteps { get; }
        public double Convergence { get; }
        public int MaxIter { get; }

        public LeStep(int subSteps, double convergence, int maxIter)
        {
            SubSteps = subSteps;
            Convergence = convergence;
            MaxIter = maxIter;
        }

        public LeStep(LeStep other)
        {
            BoundaryIds = other.BoundaryIds;
            LoadIds = other.LoadIds;
            ContactIds = other.ContactIds;
            SubSteps = other.SubSteps;
            Convergence = other.Convergence;
            MaxIter = other.MaxIter;
        }

        public void AddIds(int[] boundaryIds, int[] loadIds, int[] contactIds)
        {
            BoundaryIds = boundaryIds;
            LoadIds = loadIds;
            ContactIds = contactIds;
        }

        public string ToCnt()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!STEP, SUBSTEPS={SubSteps}, CONVERG={Convergence}, MAXITER={MaxIter}");
            foreach (int id in BoundaryIds)
            {
                sb.AppendLine($" BOUNDARY, {id}");
            }
            foreach (int id in LoadIds)
            {
                sb.AppendLine($" LOAD, {id}");
            }
            foreach (int id in ContactIds)
            {
                sb.AppendLine($" CONTACT, {id}");
            }
            return sb.ToString();
        }
    }
}
