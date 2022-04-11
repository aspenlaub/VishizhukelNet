using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication {
    public static class ApplicationContainerBuilder {
        public static async Task<ContainerBuilder> UseApplicationAsync(this ContainerBuilder builder, VishizhukelNetEmptyWindow vishizhukelNetEmptyWindow, ILogConfiguration logConfiguration) {
            await builder.UseVishizhukelNetDvinAndPeghAsync(new DummyCsArgumentPrompter(), logConfiguration);
            if (vishizhukelNetEmptyWindow == null) {
                builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer>().SingleInstance();
            } else {
                builder.RegisterInstance(vishizhukelNetEmptyWindow);
                builder.RegisterType<GuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer>().SingleInstance();
            }

            builder.RegisterType<Application.Application>().As<Application.Application>().SingleInstance();
            builder.RegisterType<ApplicationModel>().As<ApplicationModel>().As<IApplicationModel>().As<IBusy>().SingleInstance();
            builder.RegisterType<GuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            return builder;
        }
    }
}