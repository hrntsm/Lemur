using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

using Lemur;
using Lemur.Hecmw;
using Lemur.Post;

using LemurGH.Param;
using LemurGH.Type;
using LemurGH.UI.View;

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
            pManager.AddParameter(new Param_LeMesh(), "LeMesh", "LeMesh", "Output Lemur Mesh", GH_ParamAccess.item);
            pManager.AddTextParameter("Dir", "Dir", "Directory path to save results", GH_ParamAccess.item);
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

            string resultDir = string.Empty;
            if (run)
            {
                CreateDirectory(dir);
                LeAssemble leAsm = ghLeAssemble.Value;
                LeMPIType mpiType = process == 1 ? LeMPIType.Serial : LeMPIType.MSMPI;
                leAsm?.LeHecmwControl.SetMPIValues(mpiType, process);
                leAsm?.Serialize(dir);
                ExecuteAnalysis(dir, thread, mpiType, process);
                MergeResult(dir, process, leAsm.LeControl.LeStep.SubSteps);
                resultDir = dir;

                _ = new LePost(leAsm.LeMesh, dir);
                DA.SetData(0, leAsm.LeMesh);
            }
            DA.SetData(1, resultDir);
        }

        private static void CreateDirectory(string dir)
        {
            if (Directory.Exists(dir))
            {
                try
                {
                    Directory.Delete(dir, true);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            Directory.CreateDirectory(dir);
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
            partition.WaitForExit();

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
            fistr.WaitForExit();
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
            fistr.WaitForExit();
        }

        private static void MergeResult(string dir, int process, int subSteps)
        {
            string assemblePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string rmergePath = Path.Combine(assemblePath, "Lib", "fistr_serial", "rmerge.exe");
            var rmerge = new Process();
            rmerge.StartInfo.FileName = rmergePath;
            rmerge.StartInfo.WorkingDirectory = dir;
            rmerge.StartInfo.Arguments = $" -o text -n {process} -e {subSteps} lemur_rmerge.res";
            rmerge.Start();
            rmerge.WaitForExit();
        }

        public override Guid ComponentGuid => new Guid("20aab5a3-c180-4a1f-806b-4b3c16cc8f28");

        public override void CreateAttributes()
        {
            m_attributes = new UIOptimizerComponentAttributes(this);
        }

        private sealed class UIOptimizerComponentAttributes : GH_ComponentAttributes
        {
            public UIOptimizerComponentAttributes(IGH_Component component)
              : base(component)
            {
            }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas _, GH_CanvasMouseEvent e)
            {
                var window = new MainWindow();
                window.Show();
                window.Topmost = true;
                return GH_ObjectResponse.Handled;
            }
        }
    }
}
