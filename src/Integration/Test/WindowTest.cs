using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using VishizhukelNetDemoApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.ApplicationModel;

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
        await sut.RemotelyProcessTaskListAsync(process, tasks, false, (_, _) => Task.CompletedTask);
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
        await sut.RemotelyProcessTaskListAsync(process, tasks, false, (_, _) => Task.CompletedTask);
    }

    [TestMethod]
    public async Task CanGetAndSetGridItems() {
        using var sut = await CreateWindowUnderTestAsync(nameof(VishizhukelNetDemoWindow));
        var process = await sut.FindIdleProcessAsync();
        var emptyList = new List<DemoCollectionViewSourceEntity>();
        var emptyListJson = JsonConvert.SerializeObject(emptyList);
        var nonEmptyList = new List<DemoCollectionViewSourceEntity>
        {
            new() { Date = new DateTime(2021, 7, 30), Name = "Decreased", Balance = 2404.40 },
            new() { Date = new DateTime(2021, 7, 29), Name = "Increased", Balance = 2707.70 },
            new() { Date = new DateTime(2021, 7, 28), Name = "Unchanged", Balance = 2407.70 }
        };
        var nonEmptyListJson = JsonConvert.SerializeObject(nonEmptyList);
        var tasks = new List<ControllableProcessTask>
        {
            sut.CreateVerifyValueTask(process, nameof(VishizhukelNetDemoApplicationModel.Theta), emptyListJson),
            sut.CreateSetValueTask(process, nameof(VishizhukelNetDemoApplicationModel.Theta), nonEmptyListJson),
            sut.CreateVerifyValueTask(process, nameof(VishizhukelNetDemoApplicationModel.Theta), nonEmptyListJson),
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks, false, (_, _) => Task.CompletedTask);
    }
}