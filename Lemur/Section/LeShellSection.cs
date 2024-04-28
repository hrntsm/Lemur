using System;

namespace Lemur.Section
{
    [Serializable]
    public class LeShellSection
    {
        public double Thickness { get; }
        public int NumIntegrationPoints { get; }

        public LeShellSection(double thickness, int numIntegrationPoints)
        {
            Thickness = thickness;
            NumIntegrationPoints = numIntegrationPoints;
        }
    }
}
