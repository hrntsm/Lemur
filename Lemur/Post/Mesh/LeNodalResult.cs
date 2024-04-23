using System;
using System.Collections.Generic;

namespace Lemur.Post.Mesh
{
    [Serializable]
    public class LeNodalResult
    {
        public int StepNumber { get; private set; }
        public Dictionary<string, double[]> NodalData { get; private set; }

        public LeNodalResult(int stepNumber, Dictionary<string, double[]> nodalData)
        {
            StepNumber = stepNumber;
            NodalData = nodalData;
        }
    }
}
