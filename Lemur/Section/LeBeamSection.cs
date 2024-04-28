using System;

namespace Lemur.Section
{
    [Serializable]
    public class LeBeamSection
    {
        public double[] Coordinates { get; } = new double[3];
        public double Area { get; }
        public double Iyy { get; }
        public double Izz { get; }
        public double Jx { get; }

        public LeBeamSection(double[] coordinates, double area, double iyy, double izz, double jx)
        {
            if (coordinates.Length != 3)
            {
                throw new ArgumentException("Coordinates must have 3 elements.");
            }
            Coordinates = coordinates;
            Area = area;
            Iyy = iyy;
            Izz = izz;
            Jx = jx;
        }
    }
}
