using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using Lemur;
using Lemur.Control;
using Lemur.Hecmw;
using Lemur.Mesh;
using Lemur.Mesh.Group;
using Lemur.Section;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component
{
    public class LeAssembleModel : GH_Component
    {
        private LeMesh _leMesh;
        private LeControl _leCnt;

        public LeAssembleModel()
          : base("LeAssembleModel", "ConLeAsm",
            "Construct Lemur Assemble",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "Input Lemur Mesh", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeGroup(), "LeGroup", "LeGrp", "Input Lemur Group settings", GH_ParamAccess.list);
            pManager.AddParameter(new Param_LeContactMesh(), "LeCntMsh", "ContactMsh", "Input Lemur Contact Mesh settings", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeSection(), "LeSection", "LeSec", "Input Lemur section settings", GH_ParamAccess.list);
            pManager.AddParameter(new Param_LeControl(), "LeControl", "LeCtrl", "Input Lemur Control settings", GH_ParamAccess.item);
            Params.Input[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeAssemble(), "LeAsm", "LeAsm", "Lemur Assemble", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeMesh ghLeMesh = null;
            var ghLeGroups = new List<GH_LeGroup>();
            GH_LeContactMesh ghLeContactMesh = null;
            var ghLeSection = new List<GH_LeSection>();
            GH_LeControl ghLeCnt = null;
            if (!DA.GetData(0, ref ghLeMesh)) return;
            if (!DA.GetDataList(1, ghLeGroups)) return;
            if (!DA.GetDataList(3, ghLeSection)) return;
            if (!DA.GetData(4, ref ghLeCnt)) return;

            DA.GetData(2, ref ghLeContactMesh);

            _leMesh = new LeMesh(ghLeMesh.Value);
            var leGroups = ghLeGroups.Select(x => x.Value).ToList();
            LeContactMesh leContactMsh = ghLeContactMesh?.Value;
            var leSecs = ghLeSection.Select(x => x.Value).ToList();
            _leCnt = new LeControl(ghLeCnt.Value);

            SetLeMeshToGroup(leGroups);
            SetLeMeshToSection(leSecs);
            SetLeControlToSection(leSecs);
            if (leContactMsh != null)
            {
                _leMesh.AddContact(leContactMsh);
            }

            var leHecmwControl = new LeHecmwControl();

            var leAsm = new LeAssemble(_leMesh, _leCnt, leHecmwControl);
            DA.SetData(0, new GH_LeAssemble(leAsm));
        }

        private void SetLeControlToSection(List<LeSection> leSecs)
        {
            _leCnt.ClearSection();
            foreach (LeSection leSec in leSecs)
            {
                _leCnt.AddSection(leSec);
            }
        }

        private void SetLeMeshToGroup(List<LeGroupBase> leGroups)
        {
            _leMesh.ClearGroup();
            foreach (LeGroupBase leGroup in leGroups.Where(leGroup => leGroup != null))
            {
                _leMesh.AddGroup(leGroup);
            }
        }

        private void SetLeMeshToSection(List<LeSection> leSecs)
        {
            _leMesh.ClearMaterial();
            foreach (LeSection leSec in leSecs.Where(leMat => leMat != null))
            {
                _leMesh.AddSection(leSec);
            }
        }

        public override Guid ComponentGuid => new Guid("e56e1bc6-a56a-4978-8575-527cf6cec17f");
    }
}
