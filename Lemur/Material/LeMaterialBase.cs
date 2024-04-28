using System;

namespace Lemur.Material
{
    [Serializable]
    public abstract class LeMaterialBase
    {
        public string Name { get; }
        public double Density { get; }
        public double YoungsModulus { get; }
        public double PoissonRatio { get; }

        public LeMaterialBase(string name, double density, double youngsModulus, double poissonRatio)
        {
            Name = name;
            Density = density;
            YoungsModulus = youngsModulus;
            PoissonRatio = poissonRatio;
        }

        public LeMaterialBase(LeMaterialBase other)
        {
            Name = other.Name;
            Density = other.Density;
            YoungsModulus = other.YoungsModulus;
            PoissonRatio = other.PoissonRatio;
        }

        public abstract LeMaterialBase Clone();
        public abstract string ToCnt();
    }
}
