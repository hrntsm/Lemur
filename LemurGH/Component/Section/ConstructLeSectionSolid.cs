using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Lemur.Material;
using Lemur.Section;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component.Section
{
    public class ConstructLeSection : GH_Component
    {
        public ConstructLeSection()
          : base("ConstructLeSectionSolid", "ConLeSection_Solid",
            "Construct Solid Lemur Section",
            "Lemur", "Section")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("EGroups", "EGrps", "Target Element Groups", GH_ParamAccess.list);
            pManager.AddParameter(new Param_LeMaterial(), "LeMaterial", "LeMat", "Material", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeSection(), "LeSection", "LeSec", "Lemur Section", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var eGroups = new List<string>();
            var ghLeMaterial = new GH_LeMaterial();
            if (!DA.GetDataList(0, eGroups)) return;
            if (!DA.GetData(1, ref ghLeMaterial)) return;

            LeMaterialBase material = ghLeMaterial.Value;
            var leSection = new LeSection(-1, eGroups.ToArray(), LeSectionType.SOLID, material);
            DA.SetData(0, new GH_LeSection(leSection));
        }

        public override Guid ComponentGuid => new Guid("963b592f-eafa-486c-b73b-1cd8efdf7843");
    }
}
