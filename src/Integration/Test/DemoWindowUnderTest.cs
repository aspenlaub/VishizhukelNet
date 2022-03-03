using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class DemoWindowUnderTest : DemoWindowUnderTestActions, IDisposable {
        private readonly IStarterAndStopper StarterAndStopper;

        public DemoWindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper roustStarterAndStopper) : base(tashAccessor) {
            StarterAndStopper = roustStarterAndStopper;
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
