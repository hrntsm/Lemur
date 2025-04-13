using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Eto.Forms;
using Eto.Serialization.Xaml;

namespace LemurGH.UI.View
{
    public class MainWindow : Form
    {
        private readonly ProgressBar progressBar;
        private readonly Button startButton;
        private readonly Button stopButton;
        private readonly Button resetButton;
        private readonly TextArea logTextArea;

        public MainWindow()
        {
            XamlReader.Load(this);

            // コントロールの参照を取得
            progressBar = FindChild<ProgressBar>("progressBar");
            startButton = FindChild<Button>("startButton");
            stopButton = FindChild<Button>("stopButton");
            resetButton = FindChild<Button>("resetButton");
            logTextArea = FindChild<TextArea>("logTextArea");

            // イベントハンドラーを設定
            startButton.Click += StartButton_Click;
            stopButton.Click += StopButton_Click;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            logTextArea.Text += "\nAnalysis started.";
            ExecuteSerial("C:/Users/hiroa/Desktop/lemur_test", -1);
            logTextArea.Text += "\nAnalysis end.";
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            logTextArea.Text += "\n処理を停止しました。";
            // 停止処理をここに実装
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            logTextArea.Text = "リセットしました。";
            // リセット処理をここに実装
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
    }
}
