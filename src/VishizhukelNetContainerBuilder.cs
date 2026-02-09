using System.Threading.Tasks;
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
    public static async Task<IServiceCollection> UseVishizhukelNetAndPeghAsync(this IServiceCollection services, string applicationName) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(services, applicationName, false);
    }

    public static async Task<IServiceCollection> UseVishizhukelNetDvinAndPeghAsync(this IServiceCollection services, string applicationName) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(services, applicationName, true);
    }

    private static async Task<IServiceCollection> UseVishizhukelNetAndPeghOptionallyDvinAsync(IServiceCollection services, string applicationName, bool useDvin) {
        if (useDvin) {
            await services.UseVishizhukelDvinAndPeghAsync(applicationName);
        } else {
            await services.UseVishizhukelAndPeghAsync(applicationName);
        }
        services.AddTransient<IButtonNameToCommandMapper, ButtonNameToCommandMapper>();
        services.AddTransient<IToggleButtonNameToHandlerMapper, ToggleButtonNameToHandlerMapper>();
        services.AddTransient<ITashAccessor, TashAccessor>();
        return services;
    }

    public static async Task<ContainerBuilder> UseVishizhukelNetAndPeghAsync(this ContainerBuilder builder, string applicationName) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(builder, applicationName, false);
    }

    public static async Task<ContainerBuilder> UseVishizhukelNetDvinAndPeghAsync(this ContainerBuilder builder, string applicationName) {
        return await UseVishizhukelNetAndPeghOptionallyDvinAsync(builder, applicationName, true);
    }

    private static async Task<ContainerBuilder> UseVishizhukelNetAndPeghOptionallyDvinAsync(ContainerBuilder builder, string applicationName, bool useDvin) {
        if (useDvin) {
            await builder.UseVishizhukelDvinAndPeghAsync(applicationName);
        } else {
            await builder.UseVishizhukelAndPeghAsync(applicationName);
        }

        builder.RegisterType<ButtonNameToCommandMapper>().As<IButtonNameToCommandMapper>().SingleInstance();
        builder.RegisterType<ToggleButtonNameToHandlerMapper>().As<IToggleButtonNameToHandlerMapper>().SingleInstance();
        builder.RegisterType<CanvasAndImageAndImageSizeAdjuster>().As<ICanvasAndImageSizeAdjuster>().SingleInstance();
        builder.RegisterType<TashAccessor>().As<ITashAccessor>();
        return builder;
    }
}