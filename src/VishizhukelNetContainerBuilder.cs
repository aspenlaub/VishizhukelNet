using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet;

public static class VishizhukelNetContainerBuilder {
    public static async Task<IServiceCollection> UseVishizhukelNetAndPeghAsync(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(services, csArgumentPrompter, false);
    }

    public static async Task<IServiceCollection> UseVishizhukelNetDvinAndPeghAsync(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(services, csArgumentPrompter, true);
    }

    private static async Task<IServiceCollection> UseVishizhukelNetAndPeghOptionallyDvinAsync(IServiceCollection services, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
        if (useDvin) {
            await services.UseVishizhukelDvinAndPeghAsync(csArgumentPrompter);
        } else {
            await services.UseVishizhukelAndPeghAsync(csArgumentPrompter);
        }
        services.AddTransient<IButtonNameToCommandMapper, ButtonNameToCommandMapper>();
        services.AddTransient<IToggleButtonNameToHandlerMapper, ToggleButtonNameToHandlerMapper>();
        services.AddTransient<ITashAccessor, TashAccessor>();
        services.AddTransient<IBasicHtmlHelper, BasicHtmlHelper>();
        return services;
    }

    public static async Task<ContainerBuilder> UseVishizhukelNetAndPeghAsync(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, ILogConfiguration logConfiguration) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(builder, csArgumentPrompter, false, logConfiguration);
    }

    public static async Task<ContainerBuilder> UseVishizhukelNetDvinAndPeghAsync(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, ILogConfiguration logConfiguration) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(builder, csArgumentPrompter, true, logConfiguration);
    }

    private static async Task<ContainerBuilder> UseVishizhukelNetAndPeghOptionallyDvinAsync(ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, bool useDvin, ILogConfiguration logConfiguration) {
        if (useDvin) {
            await builder.UseVishizhukelDvinAndPeghAsync(csArgumentPrompter);
        } else {
            await builder.UseVishizhukelAndPeghAsync(csArgumentPrompter);
        }

        builder.RegisterInstance(logConfiguration);
        builder.RegisterType<ButtonNameToCommandMapper>().As<IButtonNameToCommandMapper>().SingleInstance();
        builder.RegisterType<ToggleButtonNameToHandlerMapper>().As<IToggleButtonNameToHandlerMapper>().SingleInstance();
        builder.RegisterType<CanvasAndImageAndImageSizeAdjuster>().As<ICanvasAndImageSizeAdjuster>().SingleInstance();
        builder.RegisterType<TashAccessor>().As<ITashAccessor>();
        builder.RegisterType<BasicHtmlHelper>().As<IBasicHtmlHelper>();
        return builder;
    }
}