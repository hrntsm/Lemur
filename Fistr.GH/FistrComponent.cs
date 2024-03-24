using System;

using Grasshopper;
using Grasshopper.Kernel;

namespace FistrGH
{
    public class fistr_ghComponent : GH_Component
    {
        public fistr_ghComponent()
          : base("fistr_gh Component", "Nickname",
            "Description of component",
            "Category", "Subcategory")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Fistr", "Fistr", "Fistr object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object fistr = null;
            if (!DA.GetData(0, ref fistr)) return;

            if (fistr == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Fistr object is null");
                return;
            }


        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("26ee9dc0-a3fc-4f4c-a1aa-f200f1f58411");
    }
}
