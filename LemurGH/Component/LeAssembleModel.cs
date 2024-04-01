using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using Lemur;
using Lemur.Control;
using Lemur.Hecmw;
using Lemur.Mesh;
using Lemur.Mesh.Group;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component
{
    public class LeAssembleModel : GH_Component
    {
        public LeAssembleModel()
          : base("LeAssembleModel", "ConLeAsm",
            "Construct Lemur Assemble",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "Input Lemur Mesh", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeGroup(), "LeGrp", "LeGrp", "Input Lemur Group settings", GH_ParamAccess.list);
            pManager.AddParameter(new Param_LeContactMesh(), "LeCntMsh", "LeCntMsh", "Input Lemur Contact Mesh settings", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeMaterial(), "LeMat", "LeMat", "Input Lemur Material settings", GH_ParamAccess.list);
            pManager.AddParameter(new Param_LeControl(), "LeCnt", "LeCnt", "Input Lemur Control settings", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeAssemble(), "LeAsm", "LeAsm", "Lemur Assemble", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeMesh ghLeMesh = null;
            var ghLeGroups = new List<GH_LeGroup>();
            GH_LeContactMesh ghLeCntMsh = null;
            var ghLeMat = new List<GH_LeMaterial>();
            GH_LeControl ghLeCnt = null;
            if (!DA.GetData(0, ref ghLeMesh)) return;
            if (!DA.GetDataList(1, ghLeGroups)) return;
            if (!DA.GetData(2, ref ghLeCntMsh)) return;
            if (!DA.GetDataList(3, ghLeMat)) return;
            if (!DA.GetData(4, ref ghLeCnt)) return;

            LeMesh leMesh = ghLeMesh.Value;
            var leGroups = ghLeGroups.Select(x => x.Value).ToList();
            LeContactMesh leContactMsh = ghLeCntMsh.Value;
            var leMats = ghLeMat.Select(x => x.Value).ToList();
            LeControl leCnt = ghLeCnt.Value;

            SetLeMeshToGroup(leMesh, leGroups);
            SetLeMeshToMaterial(leMesh, leMats);
            leMesh.AddContact(leContactMsh);

            var leHecmwControl = new LeHecmwControl();

            var leAsm = new LeAssemble(leMesh, leCnt, leHecmwControl);
            DA.SetData(0, new GH_LeAssemble(leAsm));
        }

        private static void SetLeMeshToGroup(LeMesh leMesh, List<LeGroupBase> leGroups)
        {
            leMesh.ClearGroup();
            foreach (LeGroupBase leGroup in leGroups.Where(leGroup => leGroup != null))
            {
                leMesh.AddGroup(leGroup);
            }
        }

        private static void SetLeMeshToMaterial(LeMesh leMesh, List<LeMaterial> leMats)
        {
            leMesh.ClearMaterial();
            foreach (LeMaterial leMat in leMats.Where(leMat => leMat != null))
            {
                leMesh.AddMaterial(leMat);
            }
        }

        public override Guid ComponentGuid => new Guid("e56e1bc6-a56a-4978-8575-527cf6cec17f");
    }
}
