using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelCore;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet {
    public static class VishizhukelNetContainerBuilder {
        public static IServiceCollection UseVishizhukelNetAndPegh(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            services.UseVishizhukelAndPegh(csArgumentPrompter);
            services.AddTransient<IButtonNameToCommandMapper, ButtonNameToCommandMapper>();
            return services;
        }

        public static ContainerBuilder UseVishizhukelNetAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            builder.UseVishizhukelAndPegh(csArgumentPrompter);
            builder.RegisterType<ButtonNameToCommandMapper>().As<IButtonNameToCommandMapper>().SingleInstance();
            return builder;
        }
    }
}
