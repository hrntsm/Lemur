using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeSection : GH_PersistentParam<GH_LeSection>
    {
        public Param_LeSection()
         : base("LeSection", "LeSection",
          "Lemur Section",
          "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeSection> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeSection value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("6b3a4319-f3e3-41ff-a1e2-27f6105fb381");
    }
}
