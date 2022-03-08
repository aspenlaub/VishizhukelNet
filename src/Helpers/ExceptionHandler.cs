using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    public class ExceptionHandler {
        private static Exception Exception;

        private static WindowsApplication Application;

        public static async Task RunAsync(WindowsApplication application, TimeSpan interval) {
            Application = application;
            AttachEventHandlers();
            var timer = new PeriodicTimer(interval);
            var first = true;
            var synchronizationContext = SynchronizationContext.Current;
            var folder = new Folder(Path.GetTempPath()).SubFolder("AspenlaubExceptions");
            while (await timer.WaitForNextTickAsync()) {
                if (first) {
                    if (FirstIntervalRunning != null) {
                        synchronizationContext?.Send(_ => FirstIntervalRunning(null, EventArgs.Empty), null);
                    }
                    first = false;
                }

                if (IntervalPassed != null) {
                    synchronizationContext?.Send(_ => IntervalPassed(null, EventArgs.Empty), null);
                }

                if (Exception == null) { continue; }

                await ExceptionSaverAndSender.SaveUnhandledExceptionAsync(folder, Exception, nameof(application.DispatcherUnhandledException));
                TryAndExit();
            }
        }

        private static void SaveUnobservedTaskExceptionAndExit(object sender, UnobservedTaskExceptionEventArgs e) {
            DetachEventHandlers();
            e.SetObserved();
            Exception = e.Exception;
        }

        private static void SaveUnhandledAppDomainExceptionAndExit(object sender, UnhandledExceptionEventArgs e) {
            throw new NotImplementedException(nameof(SaveUnhandledAppDomainExceptionAndExit));
        }

        private static void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            DetachEventHandlers();
            e.Handled = true;
            Exception = e.Exception;
        }

        private static void TryAndExit() {
            try {
                WindowsApplication.Current.Shutdown();
            } catch {
                try {
                    Process.GetCurrentProcess().Kill();
                } catch {
                    Environment.Exit(1);
                }
            }
        }

        private static void AttachEventHandlers() {
            AppDomain.CurrentDomain.UnhandledException += SaveUnhandledAppDomainExceptionAndExit;
            TaskScheduler.UnobservedTaskException += SaveUnobservedTaskExceptionAndExit;

            if (Application == null) { return; }

            Application.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        }

        private static void DetachEventHandlers() {
            AppDomain.CurrentDomain.UnhandledException -= SaveUnhandledAppDomainExceptionAndExit;
            TaskScheduler.UnobservedTaskException -= SaveUnobservedTaskExceptionAndExit;

            if (Application == null) { return; }

            Application.DispatcherUnhandledException -= Application_DispatcherUnhandledException;
        }

        public static event EventHandler FirstIntervalRunning;
        public static event EventHandler IntervalPassed;
    }
}
