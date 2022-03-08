using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IDemoCommands {
        ICommand GammaCommand { get; set; }
        ICommand IotaCommand { get; set; }
        ICommand KappaCommand { get; set; }
    }
}
