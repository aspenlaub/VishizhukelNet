using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    [TestClass]
    public class VishizhukelNetDemoWindowTest : VishizhukelNetDemoIntegrationTestBase {
        [TestMethod]
        public async Task CanOpenDemoWindow() {
            using (await CreateDemoWindowUnderTestAsync()) { }
        }
    }
}
