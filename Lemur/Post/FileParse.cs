using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Lemur.Post
{
    public class ResFileParser
    {
        public int NNode { get; private set; }
        public int NElem { get; private set; }
        public int NnComponent { get; private set; }
        public int NeComponent { get; private set; }
        public int[] NnDof { get; private set; }
        public string[] NodeLabels { get; private set; }
        public Dictionary<int, double[]> NodeData { get; private set; }
        public int[] NeDof { get; private set; }
        public string[] ElemLabels { get; private set; }
        public Dictionary<int, double[]> ElemData { get; private set; }

        public void Parse(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                bool isData = false;
                while (!isData)
                {
                    string read = reader.ReadLine();
                    if (read == "*data")
                    {
                        isData = true;
                    }
                }

                string[] line = reader.ReadLine().Split(' ');
                NNode = int.Parse(line[0], CultureInfo.InvariantCulture);
                NElem = int.Parse(line[1], CultureInfo.InvariantCulture);

                line = reader.ReadLine().Split(' ');
                NnComponent = int.Parse(line[0], CultureInfo.InvariantCulture);
                NeComponent = int.Parse(line[1], CultureInfo.InvariantCulture);

                NnDof = reader.ReadLine().TrimEnd().Split(' ').Select(int.Parse).ToArray();
                NodeLabels = new string[NnComponent];
                for (int i = 0; i < NnComponent; i++)
                {
                    NodeLabels[i] = reader.ReadLine();
                }

                NodeData = new Dictionary<int, double[]>();
                for (int i = 0; i < NNode; i++)
                {
                    line = reader.ReadLine().TrimEnd().Split(' ');
                    int globalId = int.Parse(line[0], CultureInfo.InvariantCulture);

                    var values = new List<double>();
                    while (values.Count != NnDof.Sum())
                    {
                        IEnumerable<double> valueLine = reader.ReadLine().TrimEnd().Split(' ').Select(double.Parse);
                        values.AddRange(valueLine);
                    }
                    NodeData[globalId] = values.ToArray();
                }

                NeDof = reader.ReadLine().TrimEnd().Split(' ').Select(int.Parse).ToArray();
                ElemLabels = new string[NeComponent];
                for (int i = 0; i < NeComponent; i++)
                {
                    ElemLabels[i] = reader.ReadLine();
                }

                ElemData = new Dictionary<int, double[]>();
                for (int i = 0; i < NElem; i++)
                {
                    line = reader.ReadLine().TrimEnd().Split(' ');
                    int globalId = int.Parse(line[0], CultureInfo.InvariantCulture);

                    var values = new List<double>();
                    while (values.Count != NeDof.Sum())
                    {
                        IEnumerable<double> valueLine = reader.ReadLine().TrimEnd().Split(' ').Select(double.Parse);
                        values.AddRange(valueLine);
                    }
                    ElemData[globalId] = values.ToArray();
                }
            }
        }

        public Dictionary<int, Dictionary<string, double[]>> GetNodeDataByLabel()
        {
            var nodeDataByLabel = new Dictionary<int, Dictionary<string, double[]>>();
            foreach (KeyValuePair<int, double[]> item in NodeData)
            {
                int nodeId = item.Key;
                double[] allValues = item.Value;
                foreach (string label in NodeLabels)
                {
                    int index = Array.IndexOf(NodeLabels, label);
                    if (!nodeDataByLabel.TryGetValue(nodeId, out Dictionary<string, double[]> value))
                    {
                        value = new Dictionary<string, double[]>();
                        nodeDataByLabel[nodeId] = value;
                    }

                    int itemLength = NnDof[index];
                    int startIndex = NnDof.Take(index).Sum();
                    double[] values = new double[itemLength];
                    Array.Copy(allValues, startIndex, values, 0, NnDof[index]);
                    value[label] = values;
                }
            }
            return nodeDataByLabel;
        }

        public Dictionary<int, Dictionary<string, double[]>> GetElemDataByLabel()
        {
            var elemDataByLabel = new Dictionary<int, Dictionary<string, double[]>>();
            foreach (KeyValuePair<int, double[]> item in ElemData)
            {
                int elemId = item.Key;
                double[] allValues = item.Value;
                foreach (string label in ElemLabels)
                {
                    int index = Array.IndexOf(ElemLabels, label);
                    if (!elemDataByLabel.TryGetValue(elemId, out Dictionary<string, double[]> value))
                    {
                        value = new Dictionary<string, double[]>();
                        elemDataByLabel[elemId] = value;
                    }

                    int itemLength = NeDof[index];
                    int startIndex = NeDof.Take(index).Sum();
                    double[] values = new double[itemLength];
                    Array.Copy(allValues, startIndex, values, 0, NeDof[index]);
                    value[label] = values;
                }
            }
            return elemDataByLabel;
        }
    }
}
