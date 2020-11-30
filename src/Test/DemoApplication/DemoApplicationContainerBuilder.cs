using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication {
    public static class DemoApplicationContainerBuilder {
        public static ContainerBuilder UseDummyApplication(this ContainerBuilder builder, bool forApplicationTest) {
            builder.RegisterType<Application.DemoApplication>().As<Application.DemoApplication>().SingleInstance();
            builder.RegisterType<DemoApplicationModel>().As<DemoApplicationModel>().SingleInstance();
            if (forApplicationTest) {
                builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IDemoGuiAndApplicationSynchronizer>().SingleInstance();
            } else {
                builder.RegisterType<DemoGuiAndApplicationSynchronizer>().As<IDemoGuiAndApplicationSynchronizer>().SingleInstance();
            }
            builder.RegisterType<DemoWindow>().As<DemoWindow>().SingleInstance();
            return builder;
        }
    }
}
