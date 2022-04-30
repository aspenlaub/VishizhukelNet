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
    private DispatcherTimer DispatcherTimer;
    private readonly int ProcessId;
    private readonly ITashAccessor TashAccessor;
    private readonly ITashHandler<TModel> TashHandler;
    private readonly IGuiToApplicationGate GuiToApplicationGate;

    public TashTimer(ITashAccessor tashAccessor, ITashHandler<TModel> tashHandler, IGuiToApplicationGate guiToApplicationGate) {
        ProcessId = Process.GetCurrentProcess().Id;
        TashAccessor = tashAccessor;
        TashHandler = tashHandler;
        GuiToApplicationGate = guiToApplicationGate;
    }

    public async ValueTask DisposeAsync() {
        if (DispatcherTimer == null) { return; }
        await StopTimerAndConfirmDeadAsync(true);
    }

    public async Task<bool> ConnectAndMakeTashRegistrationReturnSuccessAsync(string windowTitle) {
        IEnumerable<ControllableProcess> processes = null;
        try {
            processes = await TashAccessor.GetControllableProcessesAsync();
            // ReSharper disable once EmptyGeneralCatchClause
        } catch {
        }
        if (processes == null) {
            MessageBox.Show(Properties.Resources.CouldNotConnectToTash, windowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var statusCode = await TashAccessor.PutControllableProcessAsync(Process.GetCurrentProcess());
        if (statusCode == HttpStatusCode.Created || statusCode == HttpStatusCode.NoContent) {
            return true;
        }

        MessageBox.Show(string.Format(Properties.Resources.CouldNotMakeTashRegistration, statusCode.ToString()), windowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
    }

    public void CreateAndStartTimer(ITashTaskHandlingStatus<TModel> status) {
        DispatcherTimer = new DispatcherTimer();
        DispatcherTimer.Tick += async (_, _) => await TimerCallbackAsync(status);
        DispatcherTimer.Interval = TimeSpan.FromSeconds(7);
        DispatcherTimer.Start();
    }

    private async Task TimerCallbackAsync(ITashTaskHandlingStatus<TModel> status) {
        if (await TashHandler.UpdateTashStatusAndReturnIfIsWorkAsync(status)) {
            await GuiToApplicationGate.CallbackAsync(() => TashHandler.ProcessTashAsync(status));
        }
    }

    public async Task StopTimerAndConfirmDeadAsync(bool ignoreSocketException) {
        DispatcherTimer?.Stop();
        DispatcherTimer = null;

        if (ignoreSocketException) {
            try {
                await TashAccessor.ConfirmDeadWhileClosingAsync(ProcessId);
            } catch (SocketException) {
            }
        } else {
            await TashAccessor.ConfirmDeadWhileClosingAsync(ProcessId);
        }
    }
}