using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class NewtonsoftJsonProtocolDependencyInjectionExtensions
{
	public static TBuilder AddNewtonsoftJsonProtocol<TBuilder>(this TBuilder builder) where TBuilder : ISignalRBuilder
	{
		return builder.AddNewtonsoftJsonProtocol(delegate
		{
		});
	}

	public static TBuilder AddNewtonsoftJsonProtocol<TBuilder>(this TBuilder builder, Action<NewtonsoftJsonHubProtocolOptions> configure) where TBuilder : ISignalRBuilder
	{
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHubProtocol, NewtonsoftJsonHubProtocol>());
		builder.Services.Configure(configure);
		return builder;
	}
}
