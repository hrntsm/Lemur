using System.Collections.Generic;
using System.Globalization;

using Lemur.Post.Mesh;

namespace Lemur.Mesh
{
    public class LeNode
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public LeNodalResult[] NodalResults => _nodalResults.ToArray();

        private readonly List<LeNodalResult> _nodalResults = new List<LeNodalResult>();

        public LeNode(int id, double x, double y, double z)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
        }

        public void AddResult(LeNodalResult nodalResult)
        {
            _nodalResults.Add(nodalResult);
        }

        public void ClearResults()
        {
            _nodalResults.Clear();
        }

        public LeNode GetDeformedNode(int step, double scale)
        {
            var deformedNode = new LeNode(Id, X, Y, Z);
            foreach (LeNodalResult nodalResult in _nodalResults)
            {
                if (nodalResult.StepNumber == step)
                {
                    if (nodalResult.NodalData.TryGetValue("DISPLACEMENT", out double[] value))
                    {
                        double[] displacement = value;
                        deformedNode.X += displacement[0] * scale;
                        deformedNode.Y += displacement[1] * scale;
                        deformedNode.Z += displacement[2] * scale;
                    }
                }
            }
            return deformedNode;
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
