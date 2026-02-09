using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces;
using Autofac;
using FakeGuiAndApplicationSynchronizer = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Helpers.FakeGuiAndApplicationSynchronizer;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication;

public static class ApplicationContainerBuilder {
    public static async Task<ContainerBuilder> UseApplicationAsync(this ContainerBuilder builder, VishizhukelNetEmptyWindow vishizhukelNetEmptyWindow) {
        await builder.UseVishizhukelNetDvinAndPeghAsync("VishizhukelNet");
        if (vishizhukelNetEmptyWindow == null) {
            builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer<ApplicationModel>>().SingleInstance();
        } else {
            builder.RegisterInstance(vishizhukelNetEmptyWindow);
            builder.RegisterType<GuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer<ApplicationModel>>().SingleInstance();
        }

        builder.RegisterType<Application.Application>().As<Application.Application>().SingleInstance();
        builder.RegisterType<ApplicationModel>().As<ApplicationModel>().As<IApplicationModel>().As<IBusy>().SingleInstance();
        builder.RegisterType<GuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
        return builder;
    }
}