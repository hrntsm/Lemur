using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Grasshopper.Kernel;

using Lemur;
using Lemur.Hecmw;

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
            pManager.AddIntegerParameter("Thread", "nt", "Number of OpenMP threads. -1 means auto.", GH_ParamAccess.item, -1);
            pManager.AddIntegerParameter("Process", "np", "Number of MPI process", GH_ParamAccess.item, 1);
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
            int process = 1;
            string dir = string.Empty;
            bool run = false;
            if (!DA.GetData(0, ref ghLeAssemble)) return;
            if (!DA.GetData(1, ref thread)) return;
            if (!DA.GetData(2, ref process)) return;
            if (!DA.GetData(3, ref dir)) return;
            if (!DA.GetData(4, ref run)) return;

            if (run)
            {
                LeAssemble leAsm = ghLeAssemble.Value;
                LeMPIType mpiType = process == 1 ? LeMPIType.Serial : LeMPIType.MSMPI;
                leAsm?.LeHecmwControl.SetMPIValues(mpiType, process);
                leAsm?.Serialize(dir);
                ExecuteAnalysis(dir, thread, mpiType, process);
            }
        }

        private static void ExecuteAnalysis(string dir, int thread, LeMPIType mpiType, int process)
        {
            if (mpiType == LeMPIType.Serial)
            {
                ExecuteSerial(dir, thread);
            }
            else
            {
                ExecuteParallel(dir, thread, mpiType, process);
            }
        }

        private static void ExecuteParallel(string dir, int thread, LeMPIType mpiType, int process)
        {
            string assemblePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderName = mpiType == LeMPIType.MSMPI ? "fistr_msmpi" : "fistr_impi";

            string partitionPath = Path.Combine(assemblePath, "Lib", folderName, "hecmw_part1.exe");
            string mpiPath = Path.Combine(assemblePath, "Lib", folderName, "mpiexec.exe");
            string fistrPath = Path.Combine(assemblePath, "Lib", folderName, "fistr1.exe");

            var partition = new Process();
            partition.StartInfo.FileName = partitionPath;
            partition.StartInfo.WorkingDirectory = dir;
            partition.Start();

            var fistr = new Process();
            fistr.StartInfo.FileName = mpiPath;
            int nt = thread;
            if (thread == -1)
            {
                int numProcessor = Environment.ProcessorCount;
                nt = numProcessor / process;
            }

            fistr.StartInfo.Arguments = $"-np {process} {fistrPath} -t {nt}";
            fistr.StartInfo.WorkingDirectory = dir;
            fistr.Start();
        }

        private static void ExecuteSerial(string dir, int thread)
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
