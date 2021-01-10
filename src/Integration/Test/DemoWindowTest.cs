using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    [TestClass]
    public class DemoWindowTest : DemoIntegrationTestBase {
        [TestMethod]
        public async Task CanOpenDemoWindow() {
            using (await CreateDemoWindowUnderTestAsync()) { }
        }
    }
}
