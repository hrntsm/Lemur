using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Lemur.Mesh;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Material
{
    public class ConstructLeMaterial : GH_Component
    {
        public ConstructLeMaterial()
          : base("ConstructLeMaterial", "ConLeMat",
            "Construct Lemur Material",
            "Lemur", "Material")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Material Name", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "Dens", "Material Density", GH_ParamAccess.item);
            pManager.AddNumberParameter("YoungsModulus", "E", "Material Youngs Modulus", GH_ParamAccess.item);
            pManager.AddNumberParameter("PoissonRatio", "nu", "Material Poisson Ratio", GH_ParamAccess.item);
            pManager.AddTextParameter("TargetEGroup", "TrgEGrp", "Target Element Group Names", GH_ParamAccess.list);
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
            var targetEGroup = new List<string>();

            if (!DA.GetData(0, ref name)) return;
            if (!DA.GetData(1, ref density)) return;
            if (!DA.GetData(2, ref youngsModulus)) return;
            if (!DA.GetData(3, ref poissonRatio)) return;
            if (!DA.GetDataList(4, targetEGroup)) return;

            var leMat = new LeMaterial(name, density, youngsModulus, poissonRatio, targetEGroup.ToArray());
            DA.SetData(0, new GH_LeMaterial(leMat));
        }

        public override Guid ComponentGuid => new Guid("cf9b9dc4-fcde-453b-b244-ba8672fa8984");
    }
}
