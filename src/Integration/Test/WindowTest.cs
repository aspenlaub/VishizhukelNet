using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.GUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using VishizhukelNetDemoApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.ApplicationModel;
using VishizhukelNetWebView2ApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Entities.ApplicationModel;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test;

[TestClass]
public class WindowTest : IntegrationTestBase {
    [TestMethod]
    public async Task CanOpenAndMaximizeDemoWindow() {
        using var sut = await CreateWindowUnderTestAsync(nameof(VishizhukelNetDemoWindow));
        var process = await sut.FindIdleProcessAsync();
        var tasks = new List<ControllableProcessTask> {
            sut.CreateMaximizeTask(process)
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks);
    }

    [TestMethod]
    public async Task CanCalculateSum() {
        using var sut = await CreateWindowUnderTestAsync(nameof(VishizhukelNetDemoWindow));
        var process = await sut.FindIdleProcessAsync();
        var tasks = new List<ControllableProcessTask> {
            sut.CreateVerifyWhetherEnabledTask(process, nameof(VishizhukelNetDemoApplicationModel.Beta), false),
            sut.CreateSetValueTask(process, nameof(VishizhukelNetDemoApplicationModel.Alpha), "7"),
            sut.CreateVerifyWhetherEnabledTask(process, nameof(VishizhukelNetDemoApplicationModel.Beta), true),
            sut.CreateVerifyNumberOfItemsTask(process, nameof(VishizhukelNetDemoApplicationModel.Beta), 5),
            sut.CreateSelectBetaTask(process, "49")
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks);
    }

    [TestMethod]
    public async Task CanGetAndSetGridItems() {
        using var sut = await CreateWindowUnderTestAsync(nameof(VishizhukelNetDemoWindow));
        var process = await sut.FindIdleProcessAsync();
        var emptyList = new List<DemoCollectionViewSourceEntity>();
        var emptyListJson = JsonConvert.SerializeObject(emptyList);
        var nonEmptyList = new List<DemoCollectionViewSourceEntity> {
            new() { Date = new DateTime(2021, 7, 30), Name = "Decreased", Balance = 2404.40 },
            new() { Date = new DateTime(2021, 7, 29), Name = "Increased", Balance = 2707.70 },
            new() { Date = new DateTime(2021, 7, 28), Name = "Unchanged", Balance = 2407.70 }
        };
        var nonEmptyListJson = JsonConvert.SerializeObject(nonEmptyList);
        var tasks = new List<ControllableProcessTask> {
            sut.CreateVerifyValueTask(process, nameof(VishizhukelNetDemoApplicationModel.Theta), emptyListJson),
            sut.CreateSetValueTask(process, nameof(VishizhukelNetDemoApplicationModel.Theta), nonEmptyListJson),
            sut.CreateVerifyValueTask(process, nameof(VishizhukelNetDemoApplicationModel.Theta), nonEmptyListJson),
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks);
    }

    [TestMethod]
    public async Task WebViewFlowIsCorrect() {
        var logFileName = ApplicationLogger.LogFileName;
        using var sut = await CreateWindowUnderTestAsync(nameof(VishizhukelNetWebView2Window));
        var process = await sut.FindIdleProcessAsync();
        if (File.Exists(logFileName)) {
            File.Delete(logFileName);
        }
        var tasks = new List<ControllableProcessTask> {
            sut.CreateSetValueTask(process, nameof(VishizhukelNetWebView2ApplicationModel.WebViewUrl), "http://localhost/"),
            sut.CreatePressButtonTask(process, nameof(VishizhukelNetWebView2ApplicationModel.GoToUrl))
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks);
        Assert.IsTrue(File.Exists(logFileName));
        var actualLines = File.ReadLines(logFileName).ToList();
        Assert.IsTrue(actualLines.Count > 2, $"Got only {actualLines.Count} actual log line/-s");
        var startTime = DateTime.Parse(actualLines[0].Substring(0, actualLines[0].IndexOf(' ')));
        var endTime = DateTime.Parse(actualLines[^1].Substring(0, actualLines[^1].IndexOf(' ')));
        var elapsedSeconds = (endTime - startTime).TotalSeconds;
        actualLines = actualLines.Select(s => s.Substring(s.IndexOf(' '))).ToList();
        var folderName = GetType().Assembly.Location;
        folderName = folderName.Substring(0, folderName.LastIndexOf('\\'));
        var masterLogFileName = new Folder(folderName).FullName + @"\ExpectedWebViewFlowLog.txt";
        var expectedLines = File.ReadLines(masterLogFileName).ToList();
        Assert.AreEqual(expectedLines.Count, actualLines.Count, $"Expected {expectedLines.Count} log lines, got {actualLines.Count}");
        for (var i = 0; i < expectedLines.Count; i++) {
            Assert.AreEqual(expectedLines[i], actualLines[i], $"Difference in log line {i + 1}: expected '{expectedLines[i]}', got '{actualLines[i]}'");
        }
        var maxSeconds = 7;
        Assert.IsTrue(elapsedSeconds < maxSeconds, $"Expected navigation to take less than {maxSeconds} seconds, it was {elapsedSeconds}");
    }
}