using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Material
{
    public class LeMaterialPlasticMises : GH_Component
    {
        public LeMaterialPlasticMises()
          : base("LeMaterial_Plastic_Mises", "LeMat_PlaMises",
            "Construct Mises Plastic Lemur Material",
            "Lemur", "Material")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Material Name", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "Dens", "Material Density", GH_ParamAccess.item);
            pManager.AddNumberParameter("YoungsModulus", "E", "Material Youngs Modulus", GH_ParamAccess.item);
            pManager.AddNumberParameter("PoissonRatio", "nu", "Material Poisson Ratio", GH_ParamAccess.item);
            pManager.AddNumberParameter("PlasticStress", "PStress", "Material Plastic Stress", GH_ParamAccess.list);
            pManager.AddNumberParameter("PlasticStrain", "PStrain", "Material Plastic Strain", GH_ParamAccess.list);
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
            var plasticStress = new List<double>();
            var plasticStrain = new List<double>();

            if (!DA.GetData(0, ref name)) return;
            if (!DA.GetData(1, ref density)) return;
            if (!DA.GetData(2, ref youngsModulus)) return;
            if (!DA.GetData(3, ref poissonRatio)) return;
            if (!DA.GetDataList(4, plasticStress)) return;
            if (!DA.GetDataList(5, plasticStrain)) return;

            if (plasticStress.Count != plasticStrain.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Plastic Stress and Strain lists must be the same length");
                return;
            }

            var leMat = new Lemur.Material.LeMaterialPlasticMises(name, density, youngsModulus, poissonRatio, plasticStress, plasticStrain);
            DA.SetData(0, new GH_LeMaterial(leMat));
        }

        public override Guid ComponentGuid => new Guid("27712958-5970-4e23-85f2-397961667f6a");
    }
}
