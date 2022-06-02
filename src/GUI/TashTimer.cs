using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;

public class TashTimer<TModel> : IAsyncDisposable, ITashTimer<TModel> where TModel : IApplicationModelBase {
    private DispatcherTimer _DispatcherTimer;
    private readonly int _ProcessId;
    private readonly ITashAccessor _TashAccessor;
    private readonly ITashHandler<TModel> _TashHandler;
    private readonly IGuiToApplicationGate _GuiToApplicationGate;

    public TashTimer(ITashAccessor tashAccessor, ITashHandler<TModel> tashHandler, IGuiToApplicationGate guiToApplicationGate) {
        _ProcessId = Process.GetCurrentProcess().Id;
        _TashAccessor = tashAccessor;
        _TashHandler = tashHandler;
        _GuiToApplicationGate = guiToApplicationGate;
    }

    public async ValueTask DisposeAsync() {
        if (_DispatcherTimer == null) { return; }
        await StopTimerAndConfirmDeadAsync(true);
    }

    public async Task<bool> ConnectAndMakeTashRegistrationReturnSuccessAsync(string windowTitle) {
        IEnumerable<ControllableProcess> processes = null;
        try {
            processes = await _TashAccessor.GetControllableProcessesAsync();
            // ReSharper disable once EmptyGeneralCatchClause
        } catch {
        }
        if (processes == null) {
            MessageBox.Show(Properties.Resources.CouldNotConnectToTash, windowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var statusCode = await _TashAccessor.PutControllableProcessAsync(Process.GetCurrentProcess());
        if (statusCode == HttpStatusCode.Created || statusCode == HttpStatusCode.NoContent) {
            return true;
        }

        MessageBox.Show(string.Format(Properties.Resources.CouldNotMakeTashRegistration, statusCode.ToString()), windowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
    }

    public void CreateAndStartTimer(ITashTaskHandlingStatus<TModel> status) {
        _DispatcherTimer = new DispatcherTimer();
        _DispatcherTimer.Tick += async (_, _) => await TimerCallbackAsync(status);
        _DispatcherTimer.Interval = TimeSpan.FromSeconds(7);
        _DispatcherTimer.Start();
    }

    private async Task TimerCallbackAsync(ITashTaskHandlingStatus<TModel> status) {
        if (await _TashHandler.UpdateTashStatusAndReturnIfIsWorkAsync(status)) {
            await _GuiToApplicationGate.CallbackAsync(() => _TashHandler.ProcessTashAsync(status));
        }
    }

    public async Task StopTimerAndConfirmDeadAsync(bool ignoreSocketException) {
        _DispatcherTimer?.Stop();
        _DispatcherTimer = null;

        if (ignoreSocketException) {
            try {
                await _TashAccessor.ConfirmDeadWhileClosingAsync(_ProcessId);
            } catch (SocketException) {
            }
        } else {
            await _TashAccessor.ConfirmDeadWhileClosingAsync(_ProcessId);
        }
    }
}