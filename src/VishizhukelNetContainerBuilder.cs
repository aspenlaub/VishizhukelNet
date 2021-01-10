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
            services.AddTransient<ITashAccessor, TashAccessor>();
            return services;
        }

        public static ContainerBuilder UseVishizhukelNetAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelNetAndPeghOptionallyDvin(builder, csArgumentPrompter, false);
        }

        public static ContainerBuilder UseVishizhukelNetDvinAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelNetAndPeghOptionallyDvin(builder, csArgumentPrompter, true);
        }

        private static ContainerBuilder UseVishizhukelNetAndPeghOptionallyDvin(ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                builder.UseVishizhukelDvinAndPegh(csArgumentPrompter);
            } else {
                builder.UseVishizhukelAndPegh(csArgumentPrompter);
            }
            builder.RegisterType<ButtonNameToCommandMapper>().As<IButtonNameToCommandMapper>().SingleInstance();
            builder.RegisterType<TashAccessor>().As<ITashAccessor>().SingleInstance();
            return builder;
        }
    }
}
