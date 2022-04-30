using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands;

public class IotaCommand : ICommand {
    private readonly IApplicationModel Model;

    public IotaCommand(IApplicationModel model) {
        Model = model;
    }

    public async Task ExecuteAsync() {
        if (!Model.Gamma.Enabled) {
            return;
        }

        await Task.Run(() => { throw new NotImplementedException(); });
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        return await Task.FromResult(true);
    }
}