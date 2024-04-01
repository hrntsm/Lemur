using System;

using Grasshopper.Kernel;

using Lemur.Control.Contact;
using Lemur.Mesh;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Mesh
{
    public class ConstructContactPair : GH_Component
    {
        public ConstructContactPair()
          : base("ConstructContact", "ConCon",
            "Construct contact",
            "Lemur", "Mesh")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Contact pair name", GH_ParamAccess.item);
            pManager.AddTextParameter("Slave", "Slave", "Slave node", GH_ParamAccess.item);
            pManager.AddTextParameter("Master", "Master", "Master node", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Algorithm", "Alg", "0:Lagrange, 1:AugmentLagrange", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Interaction", "Int", "0:SmallSLID, 1:FiniteSLID, 2:TIED", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeContactMesh(), "ContactPair", "CMsh", "Contact mesh pair", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeContactControl(), "ContactControl", "CCnt", "Contact control", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = string.Empty;
            string slave = string.Empty;
            string master = string.Empty;
            int algorithm = 0;
            int interaction = 0;
            if (!DA.GetData(0, ref name)) return;
            if (!DA.GetData(1, ref slave)) return;
            if (!DA.GetData(2, ref master)) return;
            if (!DA.GetData(3, ref algorithm)) return;
            if (!DA.GetData(4, ref interaction)) return;

            var contactPair = new LeContactMesh(name, slave, master);
            var contactControl = new LeContactControl((LeContactAlgorithm)algorithm, (LeContactInteraction)interaction, name);
            DA.SetData(0, new GH_LeContactMesh(contactPair));
            DA.SetData(1, new GH_LeContactControl(contactControl));
        }

        public override Guid ComponentGuid => new Guid("fa2bba0d-23c6-4405-b895-489462bad1a0");
    }
}
