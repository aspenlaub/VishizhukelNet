using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application {
    public abstract class ApplicationBase<TGuiAndApplicationSynchronizer, TModel>
        where TModel : IApplicationModel
        where TGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<TModel> {
        protected readonly IButtonNameToCommandMapper ButtonNameToCommandMapper;
        protected readonly TGuiAndApplicationSynchronizer GuiAndApplicationSynchronizer;
        protected readonly TModel Model;

        protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, TGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, TModel model) {
            ButtonNameToCommandMapper = buttonNameToCommandMapper;
            GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
            Model = model;
        }

        protected abstract Task EnableOrDisableButtonsAsync();
        public abstract void RegisterTypes();

        public virtual async Task OnLoadedAsync() {
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
            await EnableOrDisableButtonsAsync();
            GuiAndApplicationSynchronizer.OnModelDataChanged();
        }

        public void IndicateBusy(bool force, Action onCursorChanged) {
            GuiAndApplicationSynchronizer.IndicateBusy(force, onCursorChanged);
        }
    }
}
