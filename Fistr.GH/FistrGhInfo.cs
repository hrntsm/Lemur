using System;
using System.Drawing;

using Grasshopper.Kernel;

namespace Fistr.GH
{
    public class FistrGHInfo : GH_AssemblyInfo
    {
        public override string Name => "fistr_gh Info";
        public override Bitmap Icon => null;
        public override string Description => "";
        public override Guid Id => new Guid("b1b73859-bac3-4605-9c40-66271b5c0486");
        public override string AuthorName => "";
        public override string AuthorContact => "";
    }
}
