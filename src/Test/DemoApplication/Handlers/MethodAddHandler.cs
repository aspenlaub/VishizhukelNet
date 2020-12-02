using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class MethodAddHandler : ToggleButtonHandlerBase<IDemoApplicationModel>, ISimpleToggleButtonHandler {
        private readonly DeltaTextHandler vDeltaTextHandler;

        public MethodAddHandler(IDemoApplicationModel model, DeltaTextHandler deltaTextHandler) : base(model, model.MethodAdd) {
            vDeltaTextHandler = deltaTextHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            await vDeltaTextHandler.TextChangedAsync("");
        }
    }
}
