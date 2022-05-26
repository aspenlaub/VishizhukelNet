using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Entities;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Helpers;

public class FakeGuiAndApplicationSynchronizer : FakeGuiAndApplicationSynchronizerBase, IGuiAndWebViewApplicationSynchronizer<ApplicationModel> {
    public ApplicationModel Model { get; }

    public FakeGuiAndApplicationSynchronizer(ApplicationModel model) {
        Model = model;
    }

    public async Task OnModelDataChangedAsync() {
        await Task.CompletedTask;
    }
}