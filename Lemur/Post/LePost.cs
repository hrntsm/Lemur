using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Lemur.Mesh;

namespace Lemur.Post
{
    public class LePost
    {
        private readonly LeMesh _leMesh;
        private readonly string _dirPath;
        private readonly Dictionary<int, Dictionary<int, string>> _resFile = new Dictionary<int, Dictionary<int, string>>();

        public LePost(LeMesh leMesh, string dirPath)
        {
            _leMesh = leMesh;
            _dirPath = dirPath;

            var parser = new ResFileParser();

            string[] files = Directory.GetFiles(dirPath, "*.res.*", SearchOption.AllDirectories);

            foreach (string fileName in files)
            {
                string[] parts = fileName.Split('.');
                int meshNumber = int.Parse(parts[2], CultureInfo.InvariantCulture);
                int stepNumber = int.Parse(parts[3], CultureInfo.InvariantCulture);

                if (!_resFile.TryGetValue(stepNumber, out Dictionary<int, string> value))
                {
                    value = new Dictionary<int, string>();
                    _resFile[stepNumber] = value;
                }
                parser.Parse(fileName);
                value[meshNumber] = fileName;
            }

            foreach (KeyValuePair<int, Dictionary<int, string>> step in _resFile)
            {
                foreach (KeyValuePair<int, string> mesh in step.Value)
                {
                    parser.Parse(mesh.Value);
                    leMesh.AddNodalResult(step.Key, parser.GetNodeDataByLabel());
                    leMesh.AddElementalResult(step.Key, parser.GetElemDataByLabel());
                }
            }
        }
    }
}
