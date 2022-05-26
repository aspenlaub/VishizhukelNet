using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;

public abstract class ApplicationBase<TGuiAndApplicationSynchronizer, TModel> 
        : IGuiAndAppHandler<TModel>
            where TModel : class, IApplicationModelBase
            where TGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<TModel> {
    protected readonly IButtonNameToCommandMapper ButtonNameToCommandMapper;
    protected readonly IToggleButtonNameToHandlerMapper ToggleButtonNameToHandlerMapper;
    protected readonly TGuiAndApplicationSynchronizer GuiAndApplicationSynchronizer;
    protected readonly TModel Model;
    protected readonly IApplicationLogger ApplicationLogger;

    protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        TGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, TModel model, IApplicationLogger applicationLogger) {
        ButtonNameToCommandMapper = buttonNameToCommandMapper;
        ToggleButtonNameToHandlerMapper = toggleButtonNameToHandlerMapper;
        GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
        Model = model;
        ApplicationLogger = applicationLogger;
    }

    protected abstract Task EnableOrDisableButtonsAsync();
    protected abstract void CreateCommandsAndHandlers();

    public TModel GetModel() {
        return Model;
    }

    public virtual async Task OnLoadedAsync() {
        CreateCommandsAndHandlers();
        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
        await EnableOrDisableButtonsAsync();
        await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
    }

    public async Task SyncGuiAndAppAsync() {
        await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
    }

    public void IndicateBusy(bool force) {
        GuiAndApplicationSynchronizer.IndicateBusy(force);
    }

    public void OnWindowStateChanged(WindowState windowState) {
        Model.WindowState = windowState;
    }
}