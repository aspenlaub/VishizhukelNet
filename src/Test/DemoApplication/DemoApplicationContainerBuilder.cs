using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication {
    public static class DemoApplicationContainerBuilder {
        public static ContainerBuilder UseDemoApplication(this ContainerBuilder builder, bool forApplicationTest) {
            builder.UseVishizhukelNetAndPegh(new DummyCsArgumentPrompter());
            builder.RegisterType<Application.DemoApplication>().As<Application.DemoApplication>().SingleInstance();
            builder.RegisterType<DemoApplicationModel>().As<DemoApplicationModel>().As<IBusy>().SingleInstance();
            if (forApplicationTest) {
                builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IDemoGuiAndApplicationSynchronizer>().SingleInstance();
            } else {
                builder.RegisterType<DemoGuiAndApplicationSynchronizer>().As<IDemoGuiAndApplicationSynchronizer>().SingleInstance();
            }
            builder.RegisterType<DemoWindow>().As<DemoWindow>().SingleInstance();
            builder.RegisterType<DemoGuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            return builder;
        }
    }
}
