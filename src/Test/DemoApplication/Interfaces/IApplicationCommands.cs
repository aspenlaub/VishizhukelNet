using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

public interface IApplicationCommands {
    ICommand GammaCommand { get; set; }
    ICommand IotaCommand { get; set; }
    ICommand KappaCommand { get; set; }
}