using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class DemoStarterAndStopper : IStarterAndStopper {
        private const string DemoName = "Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test";

        public void Start() {
            var executableFile = typeof(DemoWindowUnderTest).Assembly.Location
                .Replace(@"\Integration\Test\", @"\Test\")
                .Replace("Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test.dll", DemoName + ".exe");


            if (!File.Exists(executableFile)) {
                throw new Exception("File '" + executableFile + "' does not exist");
            }

            Stop();

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = executableFile,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(executableFile) ?? ""
                }
            };
            process.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (Process.GetProcessesByName(DemoName).Length != 1) {
                throw new Exception("Demo process could not be started");
            }
        }

        public void Stop() {
            bool again;
            var attempts = 10;
            do {
                again = false;
                try {
                    foreach (var process in Process.GetProcessesByName(DemoName)) {
                        process.Kill();
                    }
                } catch {
                    again = --attempts >= 0;
                }
            } while (again);
        }
    }
}