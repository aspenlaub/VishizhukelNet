using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;

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
            return (s, e) => {
                SaveUnhandledExceptionAndExit(application, folder, e.Exception, "TaskScheduler.UnobservedTaskException");
            };
        }

        private static DispatcherUnhandledExceptionEventHandler SaveUnhandledDispatchedExceptionAndExit(System.Windows.Application application, IFolder folder) {
            return (s, e) => {
                SaveUnhandledExceptionAndExit(application, folder, e.Exception, "Application.Current.DispatcherUnhandledException");
            };
        }

        private static UnhandledExceptionEventHandler SaveUnhandledAppDomainExceptionAndExit(System.Windows.Application application, IFolder folder) {
            return (s, e) => {
                SaveUnhandledExceptionAndExit(application, folder, (Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
            };
        }

        private static void SaveUnhandledExceptionAndExit(System.Windows.Application application, IFolder folder, Exception e, string source) {
            AppDomain.CurrentDomain.UnhandledException -= SaveUnhandledAppDomainExceptionAndExit(application, folder);
            application.DispatcherUnhandledException -= SaveUnhandledDispatchedExceptionAndExit(application, folder);
            TaskScheduler.UnobservedTaskException -= SaveUnobservedTaskExceptionAndExit(application, folder);
            ExceptionSaverAndSender.SaveUnhandledException(folder, e, source);
            TryAndExit();
        }

        protected static void TryAndExit() {
            try {
                Process.GetCurrentProcess().Kill();
            } catch {
                try {
                    Environment.Exit(1);
                } catch {
                    while (true) {
                        Thread.Sleep(TimeSpan.FromHours(1));
                    }
                }
            }
        }
    }
}
