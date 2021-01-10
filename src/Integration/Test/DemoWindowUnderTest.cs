using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class DemoWindowUnderTest : DemoWindowUnderTestActions, IDisposable {
        private readonly IStarterAndStopper vStarterAndStopper;

        public DemoWindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper roustStarterAndStopper) : base(tashAccessor) {
            vStarterAndStopper = roustStarterAndStopper;
        }

        public override async Task InitializeAsync() {
            await base.InitializeAsync();
            vStarterAndStopper.Start();
        }

        public void Dispose() {
            vStarterAndStopper.Stop();
        }
    }
}
