using System.Diagnostics;

namespace Desktop_Windows
{
    public partial class Form1 : Form
    {
        private Process _process;

        public Form1()
        {
            InitializeComponent();
        }
       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                // Kill API.exe process running via Func Core Tools
                //NOTE : else port will run background and occupy port
                // Wait for the process to exit
                _process?.WaitForExit(5000); // wait for 5 seconds

                // Kill the process when the form is closing
                _process?.Kill();
            }
            catch (Exception exception)
            {
                //silent fail, as not many stable possibilities
            }

        }

        private void relaunchButton_Click(object sender, EventArgs e)
        {

            StartAzureFunctionsCli();

        }

        private void StartAzureFunctionsCli()
        {
            string apiBuildPath = Path.Combine(Application.StartupPath, "api-build");
            string funcExecPath = Path.Combine(Application.StartupPath, "Azure.Functions.Cli", "func.exe");

            _process = new Process();
            _process.StartInfo.WorkingDirectory = apiBuildPath; // Set working directory to apiBuildPath
            _process.StartInfo.FileName = funcExecPath;
            _process.StartInfo.Arguments = $"start";
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.UseShellExecute = false;

            _process.OutputDataReceived += (sender, data) => AppendToTextBox(data.Data);
            _process.ErrorDataReceived += (sender, data) => AppendToTextBox(data.Data);

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        private void AppendToTextBox(string text)
        {
            if (serverOutput.InvokeRequired)
            {
                serverOutput.Invoke((Action<string>)AppendToTextBox, text);
            }
            else
            {
                serverOutput.AppendText(text + Environment.NewLine);
            }
        }
    }
}
