using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using Lemur.Mesh;
using Lemur.Mesh.Element;
using Lemur.Mesh.Group;

using LemurGH.Param;
using LemurGH.Type;

using Rhino.Geometry;

namespace LemurGH.Component.Group
{
    public class ConstructGroupFromSurface : GH_Component
    {
        public ConstructGroupFromSurface()
          : base("ConstructGroupFromSurface", "ConGrpSurf",
            "Construct Lemur Group from surface",
            "Lemur", "Group")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Group name", GH_ParamAccess.item, string.Empty);
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface", GH_ParamAccess.item);
            pManager.AddIntegerParameter("GroupType", "Type", "0:Node, 1:Element, 2:Surface", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Tolerance", "Tol", "Tolerance", GH_ParamAccess.item, 1e-6);
            Params.Input[0].Optional = true;
            Params.Input[3].Optional = true;
            Params.Input[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeGroup(), "Group", "Gr", "Group", GH_ParamAccess.item);
            pManager.AddGeometryParameter("Geometry", "Geom", "Preview group", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeMesh ghLeMesh = null;
            Surface surface = null;
            if (!DA.GetData(1, ref ghLeMesh)) return;
            if (!DA.GetData(2, ref surface)) return;

            string name = string.Empty;
            int type = 0;
            double tol = 1e-6;
            DA.GetData(0, ref name);
            DA.GetData(3, ref type);
            DA.GetData(4, ref tol);

            LeMesh leMesh = ghLeMesh.Value;
            LeNodeList nodes = leMesh.Nodes;
            var nodeDict = new Dictionary<int, Point3d>();
            foreach (LeNode node in nodes)
            {
                nodeDict[node.Id] = new Point3d(node.X, node.Y, node.Z);
            }

            var groupNodes = new List<int>();
            foreach (KeyValuePair<int, Point3d> p in nodeDict)
            {
                surface.ClosestPoint(p.Value, out double u, out double v);
                Point3d pt = surface.PointAt(u, v);
                double distance = new Line(p.Value, pt).Length;
                if (distance < tol)
                {
                    groupNodes.Add(p.Key);
                }
            }

            LeGroupBase group = null;
            GeometryBase geometry = null;
            switch (type)
            {
                case 0:
                    group = new NGroup(name, groupNodes.ToArray());
                    geometry = Utils.Preview.LeNodeToRhinoPointCloud(leMesh, ((NGroup)group).Ids);
                    break;
                case 1:
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Element group is not implemented yet.");
                    break;
                case 2:
                    group = ComputeSurfaceGroup(name, leMesh, groupNodes.ToArray());
                    geometry = Utils.Preview.LeFaceToRhinoMesh(leMesh, ((SGroup)group).Ids);
                    break;
            }

            DA.SetData(0, group);
            DA.SetData(1, geometry);
        }

        private static SGroup ComputeSurfaceGroup(string name, LeMesh leMesh, int[] targetNodeIds)
        {
            var faces = new List<(int, int)>();
            Dictionary<int, (int, int)[]> nf = leMesh.NodeFaces;
            var fn = new Dictionary<(int, int), List<int>>();

            foreach (int nodeId in targetNodeIds)
            {
                if (nf.TryGetValue(nodeId, out (int, int)[] value))
                {
                    foreach ((int, int) v in value)
                    {
                        if (!fn.TryGetValue(v, out List<int> nodes))
                        {
                            nodes = new List<int>();
                            fn[v] = nodes;
                        }
                        nodes.Add(nodeId);
                    }
                }
            }

            var elements = new List<LeElementBase>();
            foreach (LeElementList elementList in leMesh.Elements)
            {
                elements.AddRange(elementList);
            }

            foreach (KeyValuePair<(int, int), List<int>> p in fn)
            {
                LeElementBase elem = elements.FirstOrDefault(e => e.Id == p.Key.Item1);
                if (elem is LeSolidElementBase solid && solid.FaceToNodes(p.Key.Item2).Length == p.Value.Count)
                {
                    faces.Add(p.Key);
                }
            }

            return new SGroup(name, faces.ToArray());
        }

        public override Guid ComponentGuid => new Guid("4d94af90-ebab-4f0e-a601-8f6d468eb240");
    }
}
