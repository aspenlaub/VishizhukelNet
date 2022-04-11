using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class WindowUnderTest : WindowUnderTestActions, IDisposable {
        private readonly IStarterAndStopper StarterAndStopper;

        public WindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper starterAndStopper) : base(tashAccessor) {
            StarterAndStopper = starterAndStopper;
        }

        public override async Task InitializeAsync() {
            await base.InitializeAsync();
            StarterAndStopper.Start();
        }

        public void Dispose() {
            StarterAndStopper.Stop();
        }
    }
}
