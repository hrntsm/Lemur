using System;

using Grasshopper.Kernel;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Material
{
    public class LeMaterialElastic : GH_Component
    {
        public LeMaterialElastic()
          : base("LeMaterial_Elastic", "LeMat_Elastic",
            "Construct Elastic Lemur Material",
            "Lemur", "Material")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Material Name", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "Dens", "Material Density", GH_ParamAccess.item);
            pManager.AddNumberParameter("YoungsModulus", "E", "Material Youngs Modulus", GH_ParamAccess.item);
            pManager.AddNumberParameter("PoissonRatio", "nu", "Material Poisson Ratio", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeMaterial(), "LeMat", "LeMat", "Lemur Material", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = string.Empty;
            double density = 0;
            double youngsModulus = 0;
            double poissonRatio = 0;

            if (!DA.GetData(0, ref name)) return;
            if (!DA.GetData(1, ref density)) return;
            if (!DA.GetData(2, ref youngsModulus)) return;
            if (!DA.GetData(3, ref poissonRatio)) return;

            var leMat = new Lemur.Material.LeMaterialElastic(name, density, youngsModulus, poissonRatio);
            DA.SetData(0, new GH_LeMaterial(leMat));
        }

        public override Guid ComponentGuid => new Guid("cf9b9dc4-fcde-453b-b244-ba8672fa8984");
    }
}
