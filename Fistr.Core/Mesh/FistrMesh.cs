using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Fistr.Core.Mesh.Element;
using Fistr.Core.Mesh.Group;

namespace Fistr.Core.Mesh
{
    public class FistrMesh
    {
        public FistrNodeList Nodes { get; }
        public FistrElementList[] Elements
        {
            get
            {
                return _elements.ToArray();
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
        private readonly List<FistrElementList> _elements;
        private readonly List<GroupBase> _groups;

        public FistrMesh(string header)
        {
            _header = header;
            Nodes = new FistrNodeList();
            _elements = new List<FistrElementList>();
            _groups = new List<GroupBase>();
        }

        public void AddNode(FistrNode node)
        {
            Nodes.Add(node);
        }

        public void AddNodes(IEnumerable<FistrNode> nodes)
        {
            Nodes.AddRange(nodes);
        }

        public void AddElement(FistrElementBase element)
        {
            CheckNodeExistence(element);
            FistrElementList list = _elements.Find(e => e.ElementType == element.ElementType);
            if (list == null)
            {
                list = new FistrElementList(element.ElementType);
                _elements.Add(list);
            }
            list.AddElement(element);
        }

        public void AddGroup(GroupBase group)
        {
            CheckGroupExistence(group);
            _groups.Add(group);
        }

        private void CheckNodeExistence(FistrElementBase element)
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
                    foreach (int id in sGroup.Ids.Keys)
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
            foreach (FistrElementList elementList in _elements)
            {
                sb.Append(elementList.ToMsh(startId));
                startId += elementList.Count;
            }

            WriteGroup(sb);

            sb.AppendLine("!END");
            return sb.ToString();
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

        public void Serialize(string path)
        {
            File.WriteAllText(path, ToMsh());
        }
    }
}
