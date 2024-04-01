using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeStep : GH_PersistentParam<GH_LeStep>
    {
        public Param_LeStep()
         : base("LeStep", "LeStep",
          "Lemur Step",
          "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeStep> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeStep value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("f878d21c-43e1-4366-b8a8-12665ab1edad");
    }
}
