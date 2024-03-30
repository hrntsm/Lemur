using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Goo;

namespace LemurGH.Param
{
    public class Param_LeMesh : GH_PersistentParam<GH_LeMesh>
    {
        public Param_LeMesh()
         : base("LeMesh", "LeMesh",
          "Lemur Mesh",
          "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeMesh> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeMesh value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("36af0f69-edd2-4fa6-bacd-b352c7dca097");
    }
}
