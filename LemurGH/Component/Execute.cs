using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Lemur;
using Lemur.Control;
using Lemur.Mesh;

namespace LemurGH.Component
{
    public class Execute : GH_Component
    {
        public Execute()
          : base("Execute", "Execute",
            "Execute Lemur component",
            "Lemur", "Lemur")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Exe", "Exe", "Execute analysis", GH_ParamAccess.item);
            pManager.AddGenericParameter("LeMesh", "LeMesh", "Input Lemur Mesh", GH_ParamAccess.item);
            pManager.AddGenericParameter("LeCnt", "LeCnt", "Input Lemur Control settings", GH_ParamAccess.item);
            pManager.AddTextParameter("Dir", "Dir", "Directory path to save results", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result of execution", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool exe = false;
            object leMeshInput = null;
            object leCntInput = null;
            string dir = string.Empty;
            if (!DA.GetData(0, ref exe)) return;
            if (!DA.GetData(1, ref leMeshInput)) return;
            if (!DA.GetData(2, ref leCntInput)) return;
            if (!DA.GetData(3, ref dir)) return;

            if (exe)
            {
                var leMeshObj = (GH_ObjectWrapper)leMeshInput;
                var leMesh = (LeMesh)leMeshObj.Value;
                leMesh?.Serialize(dir);

                var leCntObj = (GH_ObjectWrapper)leCntInput;
                var leCnt = (LeControl)leCntObj.Value;
                leCnt?.Serialize(dir);

                var leHecmwControl = new LeHecmwControl("lemur");
                leHecmwControl.Serialize(dir);

                ExecuteAnalysis(dir);
            }
        }

        private static void ExecuteAnalysis(string dir)
        {
            string assemblePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fistrPath = Path.Combine(assemblePath, "Lib", "fistr_serial", "fistr1.exe");
            var fistr = new Process();
            fistr.StartInfo.FileName = fistrPath;
            fistr.StartInfo.WorkingDirectory = dir;
            fistr.Start();
        }

        public override Guid ComponentGuid => new Guid("20aab5a3-c180-4a1f-806b-4b3c16cc8f28");
    }
}
