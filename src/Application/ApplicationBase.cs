﻿using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application {
    public abstract class ApplicationBase<TGuiAndApplicationSynchronizer, TModel> : IGuiAndAppHandler
        where TModel : class, IApplicationModel
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
        protected abstract void CreateCommandsAndHandlers();

        public virtual async Task OnLoadedAsync() {
            CreateCommandsAndHandlers();
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
            await EnableOrDisableButtonsAsync();
            GuiAndApplicationSynchronizer.OnModelDataChanged();
        }

        public void IndicateBusy(bool force) {
            GuiAndApplicationSynchronizer.IndicateBusy(force);
        }

        public void OnWindowStateChanged(WindowState windowState) {
            Model.WindowState = windowState;
        }
    }
}
