using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Lemur.Mesh;

namespace Lemur.Post
{
    public class LoadResFile
    {
        /// <returns>key: StepId, value: (key: MeshPartId, value: FilePath)</returns>
        private readonly Dictionary<int, Dictionary<int, string>> _resFile = new Dictionary<int, Dictionary<int, string>>();

        public LoadResFile(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.res.*", SearchOption.AllDirectories);

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

                value[meshNumber] = fileName;
            }

            SetResultToLeMesh(new LeMesh("aa"));
        }

        public LeMesh SetResultToLeMesh(LeMesh leMesh)
        {
            var nodalResults = new Dictionary<int, Dictionary<string, double[]>>();
            var elementResults = new Dictionary<int, Dictionary<string, double[]>>();

            foreach (KeyValuePair<int, Dictionary<int, string>> step in _resFile)
            {
                foreach (KeyValuePair<int, string> mesh in step.Value)
                {
                    Read(mesh.Value, nodalResults, elementResults);
                }
                int aa = 2;
            }

            return null;
        }

        private static void Read(string path, Dictionary<int, Dictionary<string, double[]>> nodalResults, Dictionary<int, Dictionary<string, double[]>> elementResults)
        {
            DataSectionType dataType = DataSectionType.None;
            int nodeCount = 0;
            int elementCount = 0;
            int nodeResultCount = 0;
            int elementResultCount = 0;
            int nodeLabelCount = 0;
            int elementsLabelCount = 0;
            var nodeResultLength = new List<int>();
            var elementResultLength = new List<int>();
            var nodeResultLabels = new List<string>();
            var elementResultLabels = new List<string>();
            int targetId = 0;
            var tempResults = new List<double>();

            string[] lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < 8)
                {
                    continue;
                }

                string line = lines[i];
                if (line.StartsWith("*data", System.StringComparison.Ordinal))
                {
                    dataType = DataSectionType.NodeAndElementCount;
                    continue;
                }
                else if (dataType == DataSectionType.NodeAndElementCount)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    nodeCount = int.Parse(splitTexts[0], CultureInfo.InvariantCulture);
                    elementCount = int.Parse(splitTexts[1], CultureInfo.InvariantCulture);

                    dataType = DataSectionType.ResultLabelCount;
                }
                else if (dataType == DataSectionType.ResultLabelCount)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    nodeResultCount = int.Parse(splitTexts[0], CultureInfo.InvariantCulture);
                    elementResultCount = int.Parse(splitTexts[1], CultureInfo.InvariantCulture);

                    dataType = DataSectionType.NodeResultCountInEachLabel;
                }
                else if (dataType == DataSectionType.NodeResultCountInEachLabel)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    foreach (string s in splitTexts)
                    {
                        nodeResultLength.Add(int.Parse(s, CultureInfo.InvariantCulture));
                    }

                    dataType = DataSectionType.NodeResultLabel;
                }
                else if (dataType == DataSectionType.NodeResultLabel)
                {
                    nodeResultLabels.Add(line);
                    if (++nodeLabelCount == nodeResultCount)
                    {
                        dataType = DataSectionType.NodeId;
                    }
                }
                else if (dataType == DataSectionType.NodeId)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    targetId = int.Parse(splitTexts[0], CultureInfo.InvariantCulture);
                    tempResults.Clear();

                    dataType = DataSectionType.NodeResults;
                }
                else if (dataType == DataSectionType.NodeResults)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    tempResults.AddRange(splitTexts.Select(s => double.Parse(s, CultureInfo.InvariantCulture)));
                    int resultCountSum = nodeResultLength.Sum();
                    if (tempResults.Count == resultCountSum)
                    {
                        var values = new Dictionary<string, double[]>();
                        foreach (string label in nodeResultLabels)
                        {
                            int index = nodeResultLabels.IndexOf(label);
                            int length = nodeResultLength[index];
                            double[] value = tempResults.GetRange(0, length).ToArray();
                            values[label] = value;
                            tempResults.RemoveRange(0, length);
                        }
                        nodalResults[targetId] = values;
                        dataType = nodalResults.Count == nodeCount
                            ? DataSectionType.ElementResultCountInEachLabel
                            : DataSectionType.NodeId;
                    }
                }
                else if (dataType == DataSectionType.ElementResultCountInEachLabel)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    foreach (string text in splitTexts)
                    {
                        elementResultLength.Add(int.Parse(text, CultureInfo.InvariantCulture));
                    }

                    dataType = DataSectionType.ElementResultLabel;
                }
                else if (dataType == DataSectionType.ElementResultLabel)
                {
                    elementResultLabels.Add(line);
                    if (++elementsLabelCount == elementResultCount)
                    {
                        dataType = DataSectionType.ElementId;
                    }
                }
                else if (dataType == DataSectionType.ElementId)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    targetId = int.Parse(splitTexts[0], CultureInfo.InvariantCulture);
                    tempResults.Clear();

                    dataType = DataSectionType.ElementResults;
                }
                else if (dataType == DataSectionType.ElementResults)
                {
                    string trimmedLine = line.TrimEnd();
                    string[] splitTexts = trimmedLine.Split(' ');
                    tempResults.AddRange(splitTexts.Select(s => double.Parse(s, CultureInfo.InvariantCulture)));
                    int resultCountSum = elementResultLength.Sum();
                    if (tempResults.Count == resultCountSum)
                    {
                        var values = new Dictionary<string, double[]>();
                        foreach (string label in elementResultLabels)
                        {
                            int index = elementResultLabels.IndexOf(label);
                            int length = elementResultLength[index];
                            double[] value = tempResults.GetRange(0, length).ToArray();
                            values[label] = value;
                            tempResults.RemoveRange(0, length);
                        }
                        elementResults[targetId] = values;
                        dataType = elementResults.Count == elementCount
                            ? DataSectionType.None
                            : DataSectionType.ElementId;
                    }
                }
            }
        }
    }
}
