using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication {
    public static class DemoApplicationContainerBuilder {
        public static ContainerBuilder UseDemoApplication(this ContainerBuilder builder, VishizhukelNetDemoWindow vishizhukelNetDemoWindow, ILogConfiguration logConfiguration) {
            builder.UseVishizhukelNetDvinAndPegh(new DummyCsArgumentPrompter(), logConfiguration);
            if (vishizhukelNetDemoWindow == null) {
                builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IDemoGuiAndApplicationSynchronizer>().SingleInstance();
            } else {
                builder.RegisterInstance(vishizhukelNetDemoWindow);
                builder.RegisterType<DemoGuiAndApplicationSynchronizer>().As<IDemoGuiAndApplicationSynchronizer>().SingleInstance();
            }

            builder.RegisterType<Application.DemoApplication>().As<Application.DemoApplication>().SingleInstance();
            builder.RegisterType<DemoApplicationModel>().As<DemoApplicationModel>().As<IDemoApplicationModel>().As<IBusy>().SingleInstance();
            builder.RegisterType<DemoGuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            builder.RegisterType<ApplicationLogger>().As<IApplicationLogger>().SingleInstance();
            return builder;
        }
    }
}