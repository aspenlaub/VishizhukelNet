using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication {
    public static class ApplicationContainerBuilder {
        public static async Task<ContainerBuilder> UseApplicationAsync(this ContainerBuilder builder, VishizhukelNetWebBrowserWindow vishizhukelNetWebBrowserWindow, ILogConfiguration logConfiguration) {
            await builder.UseVishizhukelNetDvinAndPeghAsync(new DummyCsArgumentPrompter(), logConfiguration);
            if (vishizhukelNetWebBrowserWindow == null) {
                builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer>().SingleInstance();
            } else {
                builder.RegisterInstance(vishizhukelNetWebBrowserWindow);
                builder.RegisterType<GuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer>().SingleInstance();
            }

            builder.RegisterType<WebBrowserApplication.Application.Application>().As<WebBrowserApplication.Application.Application>().SingleInstance();
            builder.RegisterType<ApplicationModel>().OnActivated(e => e.Instance.UsesRealBrowser = true)
                .As<ApplicationModel>().As<IApplicationModel>().As<IBusy>().SingleInstance();
            builder.RegisterType<GuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            return builder;
        }
    }
}