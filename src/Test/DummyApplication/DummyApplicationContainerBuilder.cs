using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DummyApplication {
    public static class DummyApplicationContainerBuilder {
        public static ContainerBuilder UseDummyApplication(this ContainerBuilder builder) {
            builder.RegisterType<DummyApplication>().As<DummyApplication>().SingleInstance();
            builder.RegisterType<DummyApplicationModel>().As<DummyApplicationModel>().SingleInstance();
            builder.RegisterType<DummyGuiAndApplicationSynchronizer>().As<DummyGuiAndApplicationSynchronizer>().SingleInstance();
            builder.RegisterType<DummyWindow>().As<DummyWindow>().SingleInstance();
            return builder;
        }
    }
}
