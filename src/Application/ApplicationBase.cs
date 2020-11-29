using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application {
    public abstract class ApplicationBase {
        protected readonly IButtonNameToCommandMapper ButtonNameToCommandMapper;
        protected readonly IGuiAndApplicationSynchronizer GuiAndApplicationSynchronizer;
        protected readonly IApplicationModel Model;

        protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, IGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, IApplicationModel model) {
            ButtonNameToCommandMapper = buttonNameToCommandMapper;
            GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
            Model = model;
        }

        protected abstract Task EnableOrDisableButtonsAsync();
        public abstract void RegisterTypes();

        public async Task OnLoadedAsync() {
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
            await EnableOrDisableButtonsAsync();
            GuiAndApplicationSynchronizer.OnModelDataChanged();
        }

        public void IndicateBusy(bool force) {
            GuiAndApplicationSynchronizer.IndicateBusy(force);
        }
    }
}
