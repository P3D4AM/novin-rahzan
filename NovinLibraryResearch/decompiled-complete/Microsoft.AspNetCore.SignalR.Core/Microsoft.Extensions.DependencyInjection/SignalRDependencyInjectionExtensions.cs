using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class SignalRDependencyInjectionExtensions
{
	public static ISignalRServerBuilder AddSignalRCore(this IServiceCollection services)
	{
		services.AddSingleton<SignalRCoreMarkerService>();
		services.TryAddSingleton(typeof(HubLifetimeManager<>), typeof(DefaultHubLifetimeManager<>));
		services.TryAddSingleton(typeof(IHubProtocolResolver), typeof(DefaultHubProtocolResolver));
		services.TryAddSingleton(typeof(IHubContext<>), typeof(HubContext<>));
		services.TryAddSingleton(typeof(IHubContext<, >), typeof(HubContext<, >));
		services.TryAddSingleton(typeof(HubConnectionHandler<>), typeof(HubConnectionHandler<>));
		services.TryAddSingleton(typeof(IUserIdProvider), typeof(DefaultUserIdProvider));
		services.TryAddSingleton(typeof(HubDispatcher<>), typeof(DefaultHubDispatcher<>));
		services.TryAddScoped(typeof(IHubActivator<>), typeof(DefaultHubActivator<>));
		services.AddAuthorization();
		SignalRServerBuilder signalRServerBuilder = new SignalRServerBuilder(services);
		signalRServerBuilder.AddJsonProtocol();
		return signalRServerBuilder;
	}
}
