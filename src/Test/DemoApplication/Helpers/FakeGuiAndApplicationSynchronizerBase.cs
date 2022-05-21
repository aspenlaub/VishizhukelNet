using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers {
    public class FakeGuiAndApplicationSynchronizerBase {
        public void IndicateBusy(bool _) {
        }

        public async Task<TResult> RunScriptAsync<TResult>(IScriptStatement _) where TResult : IScriptCallResponse, new() {
            return await Task.FromResult(new TResult { Success = new YesNoInconclusive { Inconclusive = true, YesNo = false } });
        }

        public async Task WaitUntilNotNavigatingAnymoreAsync() {
            await Task.CompletedTask;
        }
    }
}
