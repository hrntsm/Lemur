using System.Collections.Generic;
using System.Globalization;

namespace Lemur.Mesh
{
    public class LeNode
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public Dictionary<string, double[]> Results { get; private set; } = new Dictionary<string, double[]>();

        public LeNode(int id, double x, double y, double z)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
        }

        public string ToMsh()
        {
            string id = Id.ToString(CultureInfo.InvariantCulture).PadLeft(9, ' ');
            string x = X.ToString("0.0000000000E+00", CultureInfo.InvariantCulture).PadLeft(19, ' ');
            string y = Y.ToString("0.0000000000E+00", CultureInfo.InvariantCulture).PadLeft(19, ' ');
            string z = Z.ToString("0.0000000000E+00", CultureInfo.InvariantCulture).PadLeft(19, ' ');
            return $"{id},{x},{y},{z}";
        }
    }
}
