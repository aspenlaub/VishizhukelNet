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
        return services;
    }

    public static async Task<ContainerBuilder> UseVishizhukelNetAndPeghAsync(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, ILogConfigurationFactory logConfigurationFactory) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(builder, csArgumentPrompter, false, logConfigurationFactory);
    }

    public static async Task<ContainerBuilder> UseVishizhukelNetDvinAndPeghAsync(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, ILogConfigurationFactory logConfigurationFactory) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(builder, csArgumentPrompter, true, logConfigurationFactory);
    }

    private static async Task<ContainerBuilder> UseVishizhukelNetAndPeghOptionallyDvinAsync(ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, bool useDvin, ILogConfigurationFactory logConfigurationFactory) {
        if (useDvin) {
            await builder.UseVishizhukelDvinAndPeghAsync(csArgumentPrompter);
        } else {
            await builder.UseVishizhukelAndPeghAsync(csArgumentPrompter);
        }

        builder.RegisterInstance(logConfigurationFactory);
        builder.RegisterType<ButtonNameToCommandMapper>().As<IButtonNameToCommandMapper>().SingleInstance();
        builder.RegisterType<ToggleButtonNameToHandlerMapper>().As<IToggleButtonNameToHandlerMapper>().SingleInstance();
        builder.RegisterType<CanvasAndImageAndImageSizeAdjuster>().As<ICanvasAndImageSizeAdjuster>().SingleInstance();
        builder.RegisterType<TashAccessor>().As<ITashAccessor>();
        return builder;
    }
}