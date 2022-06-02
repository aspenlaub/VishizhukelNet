using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test;

public class WindowUnderTest : WindowUnderTestActions, IDisposable {
    private readonly IStarterAndStopper _StarterAndStopper;
    public string WindowUnderTestClassName { get; set; } = nameof(VishizhukelNetDemoWindow);

    public WindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper starterAndStopper) : base(tashAccessor) {
        _StarterAndStopper = starterAndStopper;
    }

    public override async Task InitializeAsync() {
        await base.InitializeAsync();
        _StarterAndStopper.Start(WindowUnderTestClassName);
    }

    public void Dispose() {
        _StarterAndStopper.Stop();
    }
}