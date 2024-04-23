using System;

namespace Lemur.Control.Solution
{
    [Serializable]
    public enum LeSolutionType
    {
        STATIC,
        NLSTATIC,
        HEAT,
        EIGEN,
        DYNAMIC,
        STATICEIGEN,
        ELEMCHECK
    }
}
