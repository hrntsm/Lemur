using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Lemur.Mesh;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Post
{
    public class NodalResult : GH_Component
    {
        public NodalResult()
            : base("NodalResult", "NodalResult", "Get Nodal Result", "Lemur", "Post") { }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "LeMesh", GH_ParamAccess.item);
            pManager.AddTextParameter("Target Result Name", "Target", "Target Result Name", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Step", "Step", "Step", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("NodeIDs", "NIDs", "Node IDs", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Result", "Result", "Result", GH_ParamAccess.tree);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeMesh ghLeMesh = null;
            string target = string.Empty;
            int step = 0;
            var nodeIDs = new List<int>();
            if (!DA.GetData(0, ref ghLeMesh)) return;
            if (!DA.GetData(1, ref target)) return;
            if (!DA.GetData(2, ref step)) return;
            if (!DA.GetDataList(3, nodeIDs)) return;

            LeMesh leMesh = ghLeMesh.Value;

            var results = new GH_Structure<GH_Number>();

            foreach (int nodeID in nodeIDs)
            {
                LeNode node = leMesh.Nodes.FirstOrDefault(n => n.Id == nodeID);
                if (node == null)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Node {nodeID} not found");
                    return;
                }
                double[] result = node.NodalResults[step].NodalData[target];
                results.AppendRange(result.Select(r => new GH_Number(r)), new GH_Path(0, nodeID));
            }

            DA.SetDataTree(0, results);
        }

        public override Guid ComponentGuid => new Guid("e67ee0c9-62e3-48f2-b559-da813738beb7");
    }
}
