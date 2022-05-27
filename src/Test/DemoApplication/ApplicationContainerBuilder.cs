using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;

public static class ApplicationContainerBuilder {
    public static async Task<ContainerBuilder> UseDemoApplicationAsync(this ContainerBuilder builder,
            VishizhukelNetDemoWindow vishizhukelNetDemoWindow, ILogConfiguration logConfiguration) {
        await builder.UseVishizhukelNetDvinAndPeghAsync(new DummyCsArgumentPrompter(), logConfiguration);
        if (vishizhukelNetDemoWindow == null) {
            builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer<ApplicationModel>>().SingleInstance();
        } else {
            builder.RegisterInstance(vishizhukelNetDemoWindow);
            builder.RegisterType<GuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer<ApplicationModel>>().SingleInstance();
        }
        builder.RegisterType<Application.Application>().As<Application.Application>().SingleInstance();
        builder.RegisterType<ApplicationModel>().As<ApplicationModel>().As<IApplicationModel>().As<IBusy>().SingleInstance();
        builder.RegisterType<GuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
        builder.RegisterType<ApplicationLogger>().As<IApplicationLogger>().SingleInstance();
        return builder;
    }
}