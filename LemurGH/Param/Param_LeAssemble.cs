using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using LemurGH.Type;

namespace LemurGH.Param
{
    public class Param_LeAssemble : GH_PersistentParam<GH_LeAssemble>
    {
        public Param_LeAssemble()
          : base("LeAssemble", "LeAsm",
            "Lemur Assemble",
            "Lemur", "Params")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_LeAssemble> values) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Singular(ref GH_LeAssemble value) => GH_GetterResult.success;
        public override Guid ComponentGuid => new Guid("8deb6359-ff5a-4cd0-8a18-f7ace4d04f0e");
    }
}
