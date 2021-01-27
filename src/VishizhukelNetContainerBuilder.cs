using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelCore;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet {
    public static class VishizhukelNetContainerBuilder {
        public static IServiceCollection UseVishizhukelNetAndPegh(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelNetAndPeghOptionallyDvin(services, csArgumentPrompter, false);
        }

        public static IServiceCollection UseVishizhukelNetDvinAndPegh(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelNetAndPeghOptionallyDvin(services, csArgumentPrompter, true);
        }

        private static IServiceCollection UseVishizhukelNetAndPeghOptionallyDvin(IServiceCollection services, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                services.UseVishizhukelDvinAndPegh(csArgumentPrompter);
            } else {
                services.UseVishizhukelAndPegh(csArgumentPrompter);
            }
            services.AddTransient<IButtonNameToCommandMapper, ButtonNameToCommandMapper>();
            services.AddTransient<IToggleButtonNameToHandlerMapper, ToggleButtonNameToHandlerMapper>();
            services.AddTransient<ITashAccessor, TashAccessor>();
            return services;
        }

        public static ContainerBuilder UseVishizhukelNetAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, ILogConfiguration logConfiguration) {
            return UseVishizhukelNetAndPeghOptionallyDvin(builder, csArgumentPrompter, false, logConfiguration);
        }

        public static ContainerBuilder UseVishizhukelNetDvinAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, ILogConfiguration logConfiguration) {
            return UseVishizhukelNetAndPeghOptionallyDvin(builder, csArgumentPrompter, true, logConfiguration);
        }

        private static ContainerBuilder UseVishizhukelNetAndPeghOptionallyDvin(ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, bool useDvin, ILogConfiguration logConfiguration) {
            if (useDvin) {
                builder.UseVishizhukelDvinAndPegh(csArgumentPrompter);
            } else {
                builder.UseVishizhukelAndPegh(csArgumentPrompter);
            }

            builder.RegisterInstance(logConfiguration);
            builder.RegisterType<ButtonNameToCommandMapper>().As<IButtonNameToCommandMapper>().SingleInstance();
            builder.RegisterType<ToggleButtonNameToHandlerMapper>().As<IToggleButtonNameToHandlerMapper>().SingleInstance();
            builder.RegisterType<CanvasAndImageAndImageSizeAdjuster>().As<ICanvasAndImageSizeAdjuster>().SingleInstance();
            builder.RegisterType<TashAccessor>().As<ITashAccessor>();
            return builder;
        }
    }
}
