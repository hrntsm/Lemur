using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeContactMesh : GH_PersistentParam<GH_LeContactMesh>
    {
        public Param_LeContactMesh()
          : base("LeContactMesh", "LeCntMsh",
            "Lemur Contact Mesh",
            "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeContactMesh> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeContactMesh value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("7ece6979-f914-42ce-a19c-dda7437f2bc8");
    }
}
