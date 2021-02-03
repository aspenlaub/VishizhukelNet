using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class DemoStarterAndStopper : StarterAndStopperBase {
        protected override string ProcessName => "Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test";
        protected override List<string> AdditionalProcessNamesToStop => new List<string>();
        protected override string ExecutableFile() {
            return typeof(DemoWindowUnderTest).Assembly.Location
                .Replace(@"\Integration\Test\", @"\Test\")
                .Replace("Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test.dll", ProcessName + ".exe");
        }
    }
}