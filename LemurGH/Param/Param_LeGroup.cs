using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeGroup : GH_PersistentParam<GH_LeGroup>
    {
        public Param_LeGroup()
          : base("LeGroup", "LeGrp",
            "Lemur Group",
            "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeGroup> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeGroup value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("4f9b74ae-594a-4dd1-bc1d-ebe1f5a9e7ca");
    }
}
