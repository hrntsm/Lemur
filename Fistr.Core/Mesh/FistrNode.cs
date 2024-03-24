using System.Globalization;

namespace Fistr.Core.Mesh
{
    public class FistrNode
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public FistrNode(int id, double x, double y, double z)
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
