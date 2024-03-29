﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test;

public class IntegrationTestBase {
    protected readonly IContainer Container;

    public IntegrationTestBase() {
        Container = new ContainerBuilder().RegisterForIntegrationTest().Build();
    }

    protected async Task<WindowUnderTest> CreateWindowUnderTestAsync(string windowUnderTestClassName) {
        var sut = Container.Resolve<WindowUnderTest>();
        sut.WindowUnderTestClassName = windowUnderTestClassName;
        await sut.InitializeAsync();
        var process = await sut.FindIdleProcessAsync();
        var tasks = new List<ControllableProcessTask> {
            sut.CreateResetTask(process)
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks, false, (_, _) => Task.CompletedTask);
        return sut;
    }
}