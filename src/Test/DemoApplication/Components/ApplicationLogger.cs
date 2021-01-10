using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Components {
    public class ApplicationLogger : IApplicationLogger {
        private readonly string vLogFile;

        public ApplicationLogger() {
            var folder = new Folder(Path.GetTempPath()).SubFolder(nameof(ApplicationLogger));
            folder.CreateIfNecessary();
            vLogFile = folder.FullName + @"\DemoApplication.log";
            if (File.Exists(vLogFile)) { return; }

            File.WriteAllText(vLogFile, "");
        }

        public void LogMessage(string message) {
            File.AppendAllText(vLogFile, message + "\r\n");
        }
    }
}
