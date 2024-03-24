using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;

using Rhino.Geometry;

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

    public class FistrMesh
    {
        public FistrMeshType MeshType;
        public List<Point3d> Nodes;

        public FistrMesh(FistrMeshType meshType, List<Point3d> nodes)
        {
            MeshType = meshType;
            Nodes = nodes;
        }
    }

    public enum FistrMeshType
    {
        Line111,
        Line112,
        Plane231,
        Plane232,
        Plane241,
        Plane242,
        Solid301,
        Solid341,
        Solid342,
        Solid351,
        Solid352,
        Solid361,
        Solid362,
        Interface541,
        Interface542,
        Beam611,
        Beam641,
        Shell731,
        Shell732,
        Shell741,
        Shell743,
        Shell761,
        Shell781
    }
}
