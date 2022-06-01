using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
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
    protected readonly ISimpleLogger SimpleLogger;

    protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        TGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, TModel model, ISimpleLogger simpleLogger) {
        ButtonNameToCommandMapper = buttonNameToCommandMapper;
        ToggleButtonNameToHandlerMapper = toggleButtonNameToHandlerMapper;
        GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
        Model = model;
        SimpleLogger = simpleLogger;
    }

    protected abstract Task EnableOrDisableButtonsAsync();
    protected abstract void CreateCommandsAndHandlers();

    public TModel GetModel() {
        return Model;
    }

    public virtual async Task OnLoadedAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.CreateWithRandomId(nameof(OnLoadedAsync) + "Base"))) {
            CreateCommandsAndHandlers();
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }

    public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.CreateWithRandomId(nameof(EnableOrDisableButtonsThenSyncGuiAndAppAsync)))) {
            await EnableOrDisableButtonsAsync();
            await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
        }
    }

    public async Task SyncGuiAndAppAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.CreateWithRandomId(nameof(SyncGuiAndAppAsync)))) {
            await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
        }
    }

    public void IndicateBusy(bool force) {
        GuiAndApplicationSynchronizer.IndicateBusy(force);
    }

    public void OnWindowStateChanged(WindowState windowState) {
        Model.WindowState = windowState;
    }
}