using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Lemur.Mesh.Element;
using Lemur.Mesh.Group;
using Lemur.Post.Mesh;

namespace Lemur.Mesh
{
    public class LeMesh
    {
        public LeNodeList Nodes { get; }
        public LeElementList[] Elements => _elements.ToArray();
        public LeElementBase[] AllElements => _elements.SelectMany(e => e).ToArray();
        public (int elementId, int faceId)[] FaceMesh => ComputeFaceMesh();
        /// <summary>
        /// key:nodeId, value:(elementId, faceId)[]
        /// </summary>
        public Dictionary<int, (int elementId, int faceId)[]> NodeFaces { get; private set; }
        public NGroup[] NodeGroups => _groups.Where(g => g.Type == LeGroupType.Node).Cast<NGroup>().ToArray();
        public EGroup[] ElementGroups => _groups.Where(g => g.Type == LeGroupType.Element).Cast<EGroup>().ToArray();
        public SGroup[] SurfaceGroups => _groups.Where(g => g.Type == LeGroupType.Surface).Cast<SGroup>().ToArray();
        public LeMaterial[] Materials => _materials.ToArray();
        public LeContactMesh Contact { get; private set; }
        public Dictionary<int, Dictionary<string, (double min, double max)>> NodalResultSummary { get; private set; } = new Dictionary<int, Dictionary<string, (double, double)>>();

        private readonly string _header;
        private readonly List<LeElementList> _elements;
        private readonly List<LeGroupBase> _groups;
        private readonly List<LeMaterial> _materials;

        public LeMesh(string header)
        {
            _header = header;
            Nodes = new LeNodeList();
            _elements = new List<LeElementList>();
            _groups = new List<LeGroupBase>();
            _materials = new List<LeMaterial>();
        }

        public LeMesh(LeMesh other)
        {
            _header = other._header;
            Nodes = new LeNodeList(other.Nodes);
            _elements = new List<LeElementList>(other._elements);
            _groups = new List<LeGroupBase>(other._groups);
            _materials = new List<LeMaterial>(other._materials);
        }

        public void AddNode(LeNode node)
        {
            Nodes.Add(node);
        }

        public void AddNodes(IEnumerable<LeNode> nodes)
        {
            Nodes.AddRange(nodes);
        }

        public void AddElement(LeElementBase element)
        {
            CheckNodeExistence(element);
            LeElementList list = _elements.Find(e => e.ElementType == element.ElementType);
            if (list == null)
            {
                list = new LeElementList(element.ElementType);
                _elements.Add(list);
            }
            list.AddElement(element);
        }

        public void AddGroup(LeGroupBase group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            CheckGroupExistence(group);
            _groups.Add(group);
        }

        public void ClearGroup()
        {
            _groups.Clear();
        }

        public void AddMaterial(LeMaterial material)
        {
            _materials.Add(material);
        }

        public void ClearMaterial()
        {
            _materials.Clear();
        }

        public void AddContact(LeContactMesh contact)
        {
            contact.SetTypeUsingGroups(_groups.ToArray());

            Contact = contact
             ?? throw new ArgumentNullException(nameof(contact));
        }

        private void CheckNodeExistence(LeElementBase element)
        {
            IEnumerable<int> nodeIds = Nodes.Select(n => n.Id);
            foreach (int nodeId in element.NodeIds)
            {
                if (!nodeIds.Contains(nodeId))
                {
                    throw new ArgumentException($"NodeID:{nodeId} does not exist in this mesh node list.");
                }
            }
        }

        private void CheckGroupExistence(LeGroupBase group)
        {
            IEnumerable<int> nodeIds = Nodes.Select(n => n.Id);
            IEnumerable<int> elementIds = AllElements.Select(e => e.Id);

            switch (group.Type)
            {
                case LeGroupType.Node when group is NGroup nGroup:
                    foreach (int id in nGroup.Ids)
                    {
                        if (!nodeIds.Contains(id))
                        {
                            throw new ArgumentException($"NodeID:{id} does not exist in this mesh node list.");
                        }
                    }
                    break;
                case LeGroupType.Element when group is EGroup eGroup:
                    foreach (int id in eGroup.Ids)
                    {
                        if (!elementIds.Contains(id))
                        {
                            throw new ArgumentException($"ElementID:{id} does not exist in this mesh element list.");
                        }
                    }
                    break;
                case LeGroupType.Surface when group is SGroup sGroup:
                    foreach (int id in sGroup.Ids.Select(i => i.Item1))
                    {
                        if (!elementIds.Contains(id))
                        {
                            throw new ArgumentException($"ElementID:{id} does not exist in this mesh element list.");
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid group type");
            }
        }

        public string ToMsh()
        {
            var sb = new StringBuilder();
            sb.AppendLine("!HEADER");
            sb.AppendLine(" " + _header);
            sb.AppendLine(Nodes.ToMsh());
            int startId = 1;
            foreach (LeElementList elementList in _elements)
            {
                sb.AppendLine(elementList.ToMsh(startId));
                startId += elementList.Count;
            }
            AppendGroup(sb);
            sb.AppendLine(Contact?.ToMsh());
            AppendMaterial(sb);

            sb.AppendLine("!END");
            return sb.ToString();
        }

        private void AppendMaterial(StringBuilder sb)
        {
            if (_materials.Count > 0)
            {
                foreach (LeMaterial material in _materials)
                {
                    sb.AppendLine(material.ToMsh());
                }
            }
        }

        private void AppendGroup(StringBuilder sb)
        {
            if (NodeGroups != null)
            {
                foreach (LeGroupBase group in NodeGroups)
                {
                    sb.Append(group.ToMsh());
                }
            }

            if (ElementGroups != null)
            {
                foreach (LeGroupBase group in ElementGroups)
                {
                    sb.Append(group.ToMsh());
                }
            }

            if (SurfaceGroups != null)
            {
                foreach (LeGroupBase group in SurfaceGroups)
                {
                    sb.Append(group.ToMsh());
                }
            }
        }

        public void Serialize(string dir)
        {
            Serialize(dir, "lemur.msh");
        }

        public void Serialize(string dir, string name)
        {
            File.WriteAllText(Path.Combine(dir, name), ToMsh());
        }

        public void ComputeNodeFaceDataStructure()
        {
            var elements = new List<LeElementBase>();
            foreach (LeElementList elementList in _elements)
            {
                elements.AddRange(elementList);
            }

            var nodeFaces = new Dictionary<int, List<(int, int)>>();
            foreach (LeSolidElementBase element in elements.Where(e => e is LeSolidElementBase).Cast<LeSolidElementBase>())
            {
                Dictionary<int, int[]> nf = element.NodeFaces;
                foreach (KeyValuePair<int, int[]> pair in nf)
                {
                    if (!nodeFaces.TryGetValue(pair.Key, out List<(int, int)> value))
                    {
                        value = new List<(int, int)>();
                        nodeFaces[pair.Key] = value;
                    }
                    value.AddRange(pair.Value.Select(faceId => (element.Id, faceId)));
                }
            }

            NodeFaces = nodeFaces.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
        }

        /// <summary>
        /// Compute face mesh from solid elements.
        /// </summary>
        /// <returns>(element id, face id)[]</returns>
        private (int elementId, int faceId)[] ComputeFaceMesh()
        {
            var elements = new List<LeElementBase>();
            foreach (LeElementList elementList in _elements)
            {
                elements.AddRange(elementList);
            }

            var nodeFace = new Dictionary<string, List<(int, int)>>();
            foreach (LeSolidElementBase element in elements.Where(e => e is LeSolidElementBase).Cast<LeSolidElementBase>())
            {
                for (int i = 1; i <= element.FaceCount; i++)
                {
                    int[] faceNodeIds = element.FaceToNodes(i);
                    Array.Sort(faceNodeIds);
                    string key = CreateKey(faceNodeIds);
                    if (!nodeFace.TryGetValue(key, out List<(int, int)> value))
                    {
                        value = new List<(int, int)>();
                        nodeFace[key] = value;
                    }

                    value.Add((element.Id, i));
                }
            }

            var face = new List<(int, int)>();
            foreach (KeyValuePair<string, List<(int, int)>> pair in nodeFace)
            {
                if (pair.Value.Count == 1)
                {
                    face.Add(pair.Value[0]);
                }
            }

            return face.ToArray();
        }

        private static string CreateKey(int[] faceNodeIds)
        {
            var sb = new StringBuilder();
            foreach (int id in faceNodeIds)
            {
                sb.Append(id);
                sb.Append(',');
            }
            return sb.ToString();
        }

        public void Merge(LeMesh other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            int baseNodeIdMax = Nodes.Max(n => n.Id);
            int otherNodeIdMin = other.Nodes.Min(n => n.Id);
            int nodeIdDiff = baseNodeIdMax - otherNodeIdMin;
            int nodeOffset = nodeIdDiff >= 0 ? nodeIdDiff + 1 : 0;

            int baseElementIdMax = AllElements.Max(e => e.Id);
            int otherElementIdMin = other.AllElements.Min(e => e.Id);
            int elemIdDiff = baseElementIdMax - otherElementIdMin;
            int elemOffset = elemIdDiff >= 0 ? elemIdDiff + 1 : 0;

            foreach (LeNode node in other.Nodes)
            {
                Nodes.Add(new LeNode(node.Id + nodeOffset, node.X, node.Y, node.Z));
            }

            foreach (LeElementList otherElems in other._elements)
            {
                foreach (LeElementBase otherElem in otherElems)
                {
                    otherElem.ShiftIds(elemOffset, nodeOffset);
                    AddElement(otherElem);
                }
            }

            ComputeFaceMesh();
        }

        public void AddNodalResult(int stepId, Dictionary<int, Dictionary<string, double[]>> nodalResults)
        {
            var mises = new List<double>();
            foreach (Dictionary<string, double[]> pair in nodalResults.Values)
            {
                if (pair.TryGetValue("NodalMISES", out double[] value))
                {
                    mises.AddRange(value);
                }
            }

            if (NodalResultSummary.TryGetValue(stepId, out Dictionary<string, (double, double)> resultValue))
            {
                resultValue["NodalMISES"] = (mises.Min(), mises.Max());
            }
            else
            {
                NodalResultSummary[stepId] = new Dictionary<string, (double, double)> { { "NodalMISES", (mises.Min(), mises.Max()) } };
            }

            foreach (LeNode node in Nodes)
            {
                node.ClearResults();
                if (nodalResults.TryGetValue(node.Id, out Dictionary<string, double[]> value))
                {
                    node.AddResult(new LeNodalResult(stepId, value));
                }
            }
        }

        public void AddElementalResult(int stepId, Dictionary<int, Dictionary<string, double[]>> elementalResults)
        {
            foreach (LeElementBase element in AllElements)
            {
                element.ClearResults();
                if (elementalResults.TryGetValue(element.Id, out Dictionary<string, double[]> value))
                {
                    element.AddResult(new LeElementalResult(stepId, value));
                }
            }
        }
    }
}
