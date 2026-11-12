using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class SignalRDependencyInjectionExtensions
{
	public static ISignalRServerBuilder AddHubOptions<THub>(this ISignalRServerBuilder signalrBuilder, Action<HubOptions<THub>> configure) where THub : Hub
	{
		signalrBuilder.Services.AddSingleton<IConfigureOptions<HubOptions<THub>>, HubOptionsSetup<THub>>();
		signalrBuilder.Services.Configure(configure);
		return signalrBuilder;
	}

	public static ISignalRServerBuilder AddSignalR(this IServiceCollection services)
	{
		services.AddConnections();
		services.AddSingleton<SignalRMarkerService>();
		services.AddSingleton<IConfigureOptions<HubOptions>, HubOptionsSetup>();
		return services.AddSignalRCore();
	}

	public static ISignalRServerBuilder AddSignalR(this IServiceCollection services, Action<HubOptions> configure)
	{
		return services.Configure(configure).AddSignalR();
	}
}
