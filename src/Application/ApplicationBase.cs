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
    protected readonly ILogConfigurationFactory LogConfigurationFactory;

    protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        TGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, TModel model, ISimpleLogger simpleLogger, ILogConfigurationFactory logConfigurationFactory) {
        ButtonNameToCommandMapper = buttonNameToCommandMapper;
        ToggleButtonNameToHandlerMapper = toggleButtonNameToHandlerMapper;
        GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
        Model = model;
        SimpleLogger = simpleLogger;
        LogConfigurationFactory = logConfigurationFactory;
    }

    protected abstract Task EnableOrDisableButtonsAsync();
    protected abstract void CreateCommandsAndHandlers();

    public TModel GetModel() {
        return Model;
    }

    public virtual async Task OnLoadedAsync() {
        var logConfiguration = LogConfigurationFactory.Create();
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(OnLoadedAsync) + "Base", logConfiguration.LogId))) {
            CreateCommandsAndHandlers();
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }

    public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
        var logConfiguration = LogConfigurationFactory.Create();
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(EnableOrDisableButtonsThenSyncGuiAndAppAsync), logConfiguration.LogId))) {
            await EnableOrDisableButtonsAsync();
            await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
        }
    }

    public async Task SyncGuiAndAppAsync() {
        var logConfiguration = LogConfigurationFactory.Create();
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(SyncGuiAndAppAsync), logConfiguration.LogId))) {
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