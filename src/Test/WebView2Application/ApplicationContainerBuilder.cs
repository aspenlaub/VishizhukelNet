using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;
using Autofac;
using FakeGuiAndApplicationSynchronizer = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Helpers.FakeGuiAndApplicationSynchronizer;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application {
    public static class ApplicationContainerBuilder {
        public static async Task<ContainerBuilder> UseApplicationAsync(this ContainerBuilder builder, VishizhukelNetWebView2Window vishizhukelNetWebView2Window, ILogConfiguration logConfiguration) {
            await builder.UseVishizhukelNetDvinAndPeghAsync(new DummyCsArgumentPrompter(), logConfiguration);
            if (vishizhukelNetWebView2Window == null) {
                builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer>().SingleInstance();
            } else {
                builder.RegisterInstance(vishizhukelNetWebView2Window);
                builder.RegisterType<GuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer>().SingleInstance();
            }

            builder.RegisterType<Application.Application>().As<Application.Application>().SingleInstance();
            builder.RegisterType<ApplicationModel>().OnActivated(e => e.Instance.UsesRealBrowserOrView = true)
                .As<ApplicationModel>().As<IApplicationModel>().As<IBusy>().SingleInstance();
            builder.RegisterType<GuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            builder.RegisterType<FakeApplicationLogger>().As<IApplicationLogger>().SingleInstance();
            return builder;
        }
    }
}