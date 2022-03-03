using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    // ReSharper disable once UnusedMember.Global
    public class ExceptionHandlerUpSetter {
        // ReSharper disable once UnusedMember.Global
        public static void SetUp(System.Windows.Application application) {
            var folder = new Folder(Path.GetTempPath()).SubFolder("AspenlaubExceptions");
            AppDomain.CurrentDomain.UnhandledException += SaveUnhandledAppDomainExceptionAndExit(application, folder);
            application.DispatcherUnhandledException += SaveUnhandledDispatchedExceptionAndExit(application, folder);
            TaskScheduler.UnobservedTaskException += SaveUnobservedTaskExceptionAndExit(application, folder);
        }

        private static EventHandler<UnobservedTaskExceptionEventArgs> SaveUnobservedTaskExceptionAndExit(System.Windows.Application application, IFolder folder) {
            return (_, e) => {
                SaveUnhandledExceptionAndExitAsync(application, folder, e.Exception, "TaskScheduler.UnobservedTaskException").Wait();
            };
        }

        private static DispatcherUnhandledExceptionEventHandler SaveUnhandledDispatchedExceptionAndExit(System.Windows.Application application, IFolder folder) {
            return (_, e) => {
                SaveUnhandledExceptionAndExitAsync(application, folder, e.Exception, "Application.Current.DispatcherUnhandledException").Wait();
            };
        }

        private static UnhandledExceptionEventHandler SaveUnhandledAppDomainExceptionAndExit(System.Windows.Application application, IFolder folder) {
            return (_, e) => {
                SaveUnhandledExceptionAndExitAsync(application, folder, (Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException").Wait();
            };
        }

        private static async Task SaveUnhandledExceptionAndExitAsync(System.Windows.Application application, IFolder folder, Exception e, string source) {
            AppDomain.CurrentDomain.UnhandledException -= SaveUnhandledAppDomainExceptionAndExit(application, folder);
            application.DispatcherUnhandledException -= SaveUnhandledDispatchedExceptionAndExit(application, folder);
            TaskScheduler.UnobservedTaskException -= SaveUnobservedTaskExceptionAndExit(application, folder);
            await ExceptionSaverAndSender.SaveUnhandledExceptionAsync(folder, e, source);
            TryAndExit();
        }

        protected static void TryAndExit() {
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
    }
}
