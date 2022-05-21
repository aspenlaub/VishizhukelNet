using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Helpers;

public class FakeGuiAndApplicationSynchronizer : FakeGuiAndApplicationSynchronizerBase, IGuiAndApplicationSynchronizer {
    public IApplicationModel Model { get; }

    public FakeGuiAndApplicationSynchronizer(IApplicationModel model) {
        Model = model;
    }

    public async Task OnModelDataChangedAsync() {
        await Task.CompletedTask;
    }
}