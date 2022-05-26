using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Entities;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Helpers;

public class FakeGuiAndApplicationSynchronizer : FakeGuiAndApplicationSynchronizerBase, IGuiAndApplicationSynchronizer<ApplicationModel> {
    public ApplicationModel Model { get; }

    public FakeGuiAndApplicationSynchronizer(ApplicationModel model) {
        Model = model;
    }

    public async Task OnModelDataChangedAsync() {
        await Task.CompletedTask;
    }
}