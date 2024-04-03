using System;
using System.Linq;

using Grasshopper.Kernel;

using Lemur.Mesh.Group;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Group
{
    public class GroupEntireLeMesh : GH_Component
    {
        public GroupEntireLeMesh()
          : base("GroupEntireLeMesh", "GrpEntire",
            "Group all Lemur Mesh components",
            "Lemur", "Group")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the group", GH_ParamAccess.item);
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "Input Lemur Mesh", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeGroup(), "Group", "Gr", "Group", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = string.Empty;
            GH_LeMesh ghLeMesh = null;
            if (!DA.GetData(0, ref name)) return;
            if (!DA.GetData(1, ref ghLeMesh)) return;
            Lemur.Mesh.LeMesh leMesh = ghLeMesh.Value;

            var group = new EGroup(name, leMesh.AllElements.Select(e => e.Id).ToArray());
            Message = $"EGRP:{group.Ids.Length}items";
            DA.SetData(0, new GH_LeGroup(group));
        }

        public override Guid ComponentGuid => new Guid("eede96b9-5619-4ebb-83af-c4e35984aa74");
    }
}
