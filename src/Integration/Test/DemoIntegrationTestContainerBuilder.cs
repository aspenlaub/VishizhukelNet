using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public static class DemoIntegrationTestContainerBuilder {
        public static ContainerBuilder RegisterForDemoIntegrationTest(this ContainerBuilder builder) {
            builder.UseDvinAndPegh(new DummyCsArgumentPrompter());
            builder.RegisterType<DemoStarterAndStopper>().As<IStarterAndStopper>();
            builder.RegisterType<DemoWindowUnderTest>();
            builder.RegisterType<TashAccessor>().As<ITashAccessor>();
            return builder;
        }
    }
}
