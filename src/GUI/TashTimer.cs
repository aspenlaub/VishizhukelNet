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

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI {
    public class TashTimer<TModel> : IDisposable, ITashTimer<TModel> where TModel : IApplicationModel {
        private DispatcherTimer vDispatcherTimer;
        private readonly int vProcessId;
        private readonly ITashAccessor vTashAccessor;
        private readonly ITashHandler<TModel> vTashHandler;
        private readonly IGuiToApplicationGate vGuiToApplicationGate;

        public TashTimer(ITashAccessor tashAccessor, ITashHandler<TModel> tashHandler, IGuiToApplicationGate guiToApplicationGate) {
            vProcessId = Process.GetCurrentProcess().Id;
            vTashAccessor = tashAccessor;
            vTashHandler = tashHandler;
            vGuiToApplicationGate = guiToApplicationGate;
        }

        public void Dispose() {
            if (vDispatcherTimer == null) { return; }
            StopTimerAndConfirmDead(true);
        }

        public async Task<bool> ConnectAndMakeTashRegistrationReturnSuccessAsync(string windowTitle) {
            IEnumerable<ControllableProcess> processes = null;
            try {
                processes = await vTashAccessor.GetControllableProcessesAsync();
                // ReSharper disable once EmptyGeneralCatchClause
            } catch {
            }
            if (processes == null) {
                MessageBox.Show(Properties.Resources.CouldNotConnectToTash, windowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var statusCode = await vTashAccessor.PutControllableProcessAsync(Process.GetCurrentProcess());
            if (statusCode == HttpStatusCode.Created || statusCode == HttpStatusCode.NoContent) {
                return true;
            }

            MessageBox.Show(string.Format(Properties.Resources.CouldNotMakeTashRegistration, statusCode.ToString()), windowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        public void CreateAndStartTimer(ITashTaskHandlingStatus<TModel> status) {
            vDispatcherTimer = new DispatcherTimer();
            vDispatcherTimer.Tick += async (s, e) => await TimerCallbackAsync(status);
            vDispatcherTimer.Interval = TimeSpan.FromSeconds(7);
            vDispatcherTimer.Start();
        }

        private async Task TimerCallbackAsync(ITashTaskHandlingStatus<TModel> status) {
            if (await vTashHandler.UpdateTashStatusAndReturnIfIsWorkAsync(status)) {
                await vGuiToApplicationGate.CallbackAsync(() => vTashHandler.ProcessTashAsync(status));
            }
        }

        public void StopTimerAndConfirmDead(bool ignoreSocketException) {
            vDispatcherTimer?.Stop();
            vDispatcherTimer = null;

            if (ignoreSocketException) {
                try {
                    vTashAccessor.ConfirmDeadWhileClosing(vProcessId);
                } catch (SocketException) {
                }
            } else {
                vTashAccessor.ConfirmDeadWhileClosing(vProcessId);
            }
        }
    }
}
