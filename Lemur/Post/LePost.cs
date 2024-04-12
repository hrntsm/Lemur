using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Lemur.Mesh;

namespace Lemur.Post
{
    public class LePost
    {
        private readonly Dictionary<int, string> _resFile = new Dictionary<int, string>();

        public LePost(LeMesh leMesh, string dirPath)
        {
            var parser = new ResFileParser();

            string[] files = Directory.GetFiles(dirPath, "*_rmerge.res.*", SearchOption.AllDirectories);

            foreach (string fileName in files)
            {
                string[] parts = fileName.Split('.');
                int stepNumber = int.Parse(parts[2], CultureInfo.InvariantCulture);
                _resFile[stepNumber] = fileName;
            }

            foreach (KeyValuePair<int, string> res in _resFile)
            {
                parser.Parse(res.Value);
                leMesh.AddNodalResult(res.Key, parser.GetNodeDataByLabel());
                leMesh.AddElementalResult(res.Key, parser.GetElemDataByLabel());
            }
        }
    }
}
