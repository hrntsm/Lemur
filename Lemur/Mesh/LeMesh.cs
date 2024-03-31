using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Lemur.Mesh.Element;
using Lemur.Mesh.Group;

namespace Lemur.Mesh
{
    public class LeMesh
    {
        public LeNodeList Nodes { get; }
        public LeElementList[] Elements
        {
            get
            {
                return _elements.ToArray();
            }
        }
        public (int, int)[] FaceMesh
        {
            get
            {
                return ComputeFaceMesh();
            }
        }
        public GroupBase[] NodeGroups
        {
            get
            {
                return _groups.Where(g => g.Type == GroupType.Node).ToArray();
            }
        }
        public GroupBase[] ElementGroups
        {
            get
            {
                return _groups.Where(g => g.Type == GroupType.Element).ToArray();
            }
        }
        public GroupBase[] SurfaceGroups
        {
            get
            {
                return _groups.Where(g => g.Type == GroupType.Surface).ToArray();
            }
        }

        private readonly string _header;
        private readonly List<LeElementList> _elements;
        private readonly List<GroupBase> _groups;

        public LeMesh(string header)
        {
            _header = header;
            Nodes = new LeNodeList();
            _elements = new List<LeElementList>();
            _groups = new List<GroupBase>();
        }

        public LeMesh(LeMesh other)
        {
            _header = other._header;
            Nodes = new LeNodeList(other.Nodes);
            _elements = new List<LeElementList>(other._elements);
            _groups = new List<GroupBase>(other._groups);
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

        public void AddGroup(GroupBase group)
        {
            CheckGroupExistence(group);
            _groups.Add(group);
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

        private void CheckGroupExistence(GroupBase group)
        {
            IEnumerable<int> nodeIds = Nodes.Select(n => n.Id);
            IEnumerable<int> elementIds = _elements.SelectMany(e => e.SelectMany(el => el.NodeIds));

            switch (group.Type)
            {
                case GroupType.Node when group is NGroup nGroup:
                    foreach (int id in nGroup.Ids)
                    {
                        if (!nodeIds.Contains(id))
                        {
                            throw new ArgumentException($"NodeID:{id} does not exist in this mesh node list.");
                        }
                    }
                    break;
                case GroupType.Element when group is EGroup eGroup:
                    foreach (int id in eGroup.Ids)
                    {
                        if (!elementIds.Contains(id))
                        {
                            throw new ArgumentException($"ElementID:{id} does not exist in this mesh element list.");
                        }
                    }
                    break;
                case GroupType.Surface when group is SGroup sGroup:
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
            sb.Append(Nodes.ToMsh());
            int startId = 1;
            foreach (LeElementList elementList in _elements)
            {
                sb.Append(elementList.ToMsh(startId));
                startId += elementList.Count;
            }
            WriteMaterial(sb);
            WriteGroup(sb);

            sb.AppendLine("!END");
            return sb.ToString();
        }

        private void WriteMaterial(StringBuilder sb)
        {
            sb.AppendLine("!MATERIAL, NAME=M1, ITEM=2");
            sb.AppendLine("!ITEM=1, SUBITEM=2");
            sb.AppendLine("210000.0, 0.3");
            sb.AppendLine("!ITEM=2, SUBITEM=1");
            sb.AppendLine("7.85e-6");
            foreach (LeElementList elementList in _elements)
            {
                string eGroup = elementList.ElementType + "_AUTO";
                sb.AppendLine($"!SECTION, TYPE=SOLID, EGRP={eGroup}, MATERIAL=M1");
            }
        }

        private void WriteGroup(StringBuilder sb)
        {
            if (NodeGroups != null)
            {
                foreach (GroupBase group in NodeGroups)
                {
                    sb.Append(group.ToMsh());
                }
            }

            if (ElementGroups != null)
            {
                foreach (GroupBase group in ElementGroups)
                {
                    sb.Append(group.ToMsh());
                }
            }

            if (SurfaceGroups != null)
            {
                foreach (GroupBase group in SurfaceGroups)
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

        private (int, int)[] ComputeFaceMesh()
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
    }
}
