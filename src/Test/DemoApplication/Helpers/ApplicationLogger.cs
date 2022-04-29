using System;
using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;

public class ApplicationLogger : IApplicationLogger {
    protected static string LogFileName = @"C:\Temp\DemoApplicationLogger.log";
    private static readonly object LogFileLocker = new();

    public ApplicationLogger() {
        lock (LogFileLocker) {
            if (!File.Exists(LogFileName)) { return; }

            File.WriteAllText(LogFileName, LogFileName + Environment.NewLine);
            File.Copy(LogFileName, LogFileName.Replace(".log", ".cpy"), true);
        }
    }

    public void LogMessage(string message) {
        var timeStamp = DateTime.Now;
        lock (LogFileLocker) {
            File.AppendAllText(LogFileName, timeStamp.ToString("HH:mm:ss.fff") + @" " + message + Environment.NewLine);
            try {
                File.Copy(LogFileName, LogFileName.Replace(".log", ".cpy"), true);
                // ReSharper disable once EmptyGeneralCatchClause
            } catch {
            }
        }
    }
}
