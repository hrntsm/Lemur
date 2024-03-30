using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeControl : GH_PersistentParam<GH_LeControl>
    {
        public Param_LeControl()
          : base("LeControl", "LeCnt",
            "Lemur Control settings",
            "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeControl> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeControl value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("78a83761-5f17-4121-a7d1-d6934fdf5d15");
    }
}
