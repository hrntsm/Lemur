using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeBC : GH_PersistentParam<GH_LeBC>
    {
        public Param_LeBC()
         : base("LeBC", "LeBC",
          "Lemur Boundary Condition",
          "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeBC> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeBC value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("5180882a-efd9-4aec-9b94-97ebd86b39eb");
    }
}
