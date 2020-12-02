using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class MethodMultiplyHandler : ToggleButtonHandlerBase<IDemoApplicationModel>, ISimpleToggleButtonHandler {
        private readonly DeltaTextHandler vDeltaTextHandler;

        public MethodMultiplyHandler(IDemoApplicationModel model, DeltaTextHandler deltaTextHandler) : base(model, model.MethodMultiply) {
            vDeltaTextHandler = deltaTextHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            await vDeltaTextHandler.TextChangedAsync("");
        }
    }
}
