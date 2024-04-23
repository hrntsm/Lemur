using System;
using System.Collections.Generic;

namespace Lemur.Post.Mesh
{
    [Serializable]
    public class LeElementalResult : LeNodalResult
    {
        public LeElementalResult(int stepNumber, Dictionary<string, double[]> elementalData)
         : base(stepNumber, elementalData)
        {
        }
    }
}
