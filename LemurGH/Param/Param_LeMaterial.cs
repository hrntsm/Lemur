using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeMaterial : GH_PersistentParam<GH_LeMaterial>
    {
        public Param_LeMaterial()
         : base("LeMaterial", "LeMaterial",
          "Lemur Material",
          "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeMaterial> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeMaterial value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("8e9ca91d-c39b-4a50-b97c-b331b6b6243a");
    }
}
