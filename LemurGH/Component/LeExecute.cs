using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Grasshopper.Kernel;

using Lemur;

using LemurGH.Param;
using LemurGH.Type;

namespace LemurGH.Component
{
    public class LeExecute : GH_Component
    {
        public LeExecute()
          : base("LeExecute", "LeExe",
            "Execute Lemur component",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_LeAssemble(), "LeAssemble", "LeAsm", "Input Lemur Mesh", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Thread", "Thread", "Number of OpenMP threads. -1 means auto.", GH_ParamAccess.item, -1);
            pManager.AddTextParameter("Dir", "Dir", "Directory path to save results", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "Run", "Execute analysis", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result of execution", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_LeAssemble ghLeAssemble = null;
            int thread = -1;
            string dir = string.Empty;
            bool run = false;
            if (!DA.GetData(0, ref ghLeAssemble)) return;
            if (!DA.GetData(1, ref thread)) return;
            if (!DA.GetData(2, ref dir)) return;
            if (!DA.GetData(3, ref run)) return;

            if (run)
            {
                LeAssemble leAsm = ghLeAssemble.Value;
                leAsm?.Serialize(dir);
                ExecuteAnalysis(dir, thread);
            }
        }

        private static void ExecuteAnalysis(string dir, int thread)
        {
            string assemblePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fistrPath = Path.Combine(assemblePath, "Lib", "fistr_serial", "fistr1.exe");
            var fistr = new Process();
            fistr.StartInfo.FileName = fistrPath;
            if (thread != -1)
            {
                fistr.StartInfo.Arguments = $"-t {thread}";
            }
            fistr.StartInfo.WorkingDirectory = dir;
            fistr.Start();
        }

        public override Guid ComponentGuid => new Guid("20aab5a3-c180-4a1f-806b-4b3c16cc8f28");
    }
}
