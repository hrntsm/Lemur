using System;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Lemur;
using Lemur.Control;
using Lemur.Mesh;

namespace LemurGH
{
    public class ConstructLeAssemble : GH_Component
    {
        public ConstructLeAssemble()
          : base("ConstructLeAssemble", "ConLeAsm",
            "Construct Lemur Assemble",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LeMesh", "LeMesh", "Input Lemur Mesh", GH_ParamAccess.item);
            pManager.AddGenericParameter("LeCnt", "LeCnt", "Input Lemur Control settings", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LeAsm", "LeAsm", "Lemur Assemble", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object leMeshInput = null;
            object leCntInput = null;
            if (!DA.GetData(0, ref leMeshInput)) return;
            if (!DA.GetData(1, ref leCntInput)) return;

            var leMeshObj = (GH_ObjectWrapper)leMeshInput;
            var leMesh = (LeMesh)leMeshObj.Value;

            var leCntObj = (GH_ObjectWrapper)leCntInput;
            var leCnt = (LeControl)leCntObj.Value;

            var leHecmwControl = new LeHecmwControl();

            var leAsm = new LeAssemble(leMesh, leCnt, leHecmwControl);
            DA.SetData(0, leAsm);
        }

        public override Guid ComponentGuid => new Guid("e56e1bc6-a56a-4978-8575-527cf6cec17f");
    }
}
