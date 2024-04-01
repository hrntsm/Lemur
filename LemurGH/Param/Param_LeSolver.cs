using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeSolver : GH_PersistentParam<GH_LeSolver>
    {
        public Param_LeSolver()
         : base("LeSolver", "LeSolver",
          "Lemur Solver",
          "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeSolver> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeSolver value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("c828ed9d-cbaa-495f-83d8-93e3d2c81dd5");
    }
}
