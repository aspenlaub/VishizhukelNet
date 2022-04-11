using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands {
    public class ApplicationCommands : IApplicationCommands {
        public ICommand GammaCommand { get; set; }
        public ICommand IotaCommand { get; set; }
        public ICommand KappaCommand { get; set; }
    }
}
