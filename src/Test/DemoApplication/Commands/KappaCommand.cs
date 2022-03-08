using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands {
    public class KappaCommand : ICommand {
        private readonly IDemoApplicationModel Model;

        public KappaCommand(IDemoApplicationModel model) {
            Model = model;
        }

        public async Task ExecuteAsync() {
            if (!await Task.FromResult(Model.Kappa.Enabled)) {
                return;
            }

            // Not awaited
            Task.Run(() => { throw new NotImplementedException(); });
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            return await Task.FromResult(true);
        }
    }
}
