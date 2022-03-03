using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class MethodAddHandler : ToggleButtonHandlerBase<IDemoApplicationModel> {
        private readonly DeltaTextHandler DeltaTextHandler;

        public MethodAddHandler(IDemoApplicationModel model, DeltaTextHandler deltaTextHandler) : base(model, model.MethodAdd) {
            DeltaTextHandler = deltaTextHandler;
        }

        public override async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            await DeltaTextHandler.TextChangedAsync("");
        }
    }
}
