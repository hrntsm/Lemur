using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeContactControl : GH_PersistentParam<GH_LeContactControl>
    {
        public Param_LeContactControl()
          : base("LeContactMesh", "LeCntMsh",
            "Lemur Contact Mesh",
            "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeContactControl> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeContactControl value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("c82c7f89-b06b-4c96-b5e5-f5a5dd301d5d");
    }
}
