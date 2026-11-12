using System;
using Microsoft.AspNetCore.SignalR.Client.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.SignalR.Client;

public static class HubConnectionBuilderExtensions
{
	public static IHubConnectionBuilder ConfigureLogging(this IHubConnectionBuilder hubConnectionBuilder, Action<ILoggingBuilder> configureLogging)
	{
		hubConnectionBuilder.Services.AddLogging(configureLogging);
		return hubConnectionBuilder;
	}

	public static IHubConnectionBuilder WithAutomaticReconnect(this IHubConnectionBuilder hubConnectionBuilder)
	{
		hubConnectionBuilder.Services.AddSingleton((IRetryPolicy)new DefaultRetryPolicy());
		return hubConnectionBuilder;
	}

	public static IHubConnectionBuilder WithAutomaticReconnect(this IHubConnectionBuilder hubConnectionBuilder, TimeSpan[] reconnectDelays)
	{
		hubConnectionBuilder.Services.AddSingleton((IRetryPolicy)new DefaultRetryPolicy(reconnectDelays));
		return hubConnectionBuilder;
	}

	public static IHubConnectionBuilder WithAutomaticReconnect(this IHubConnectionBuilder hubConnectionBuilder, IRetryPolicy retryPolicy)
	{
		hubConnectionBuilder.Services.AddSingleton(retryPolicy);
		return hubConnectionBuilder;
	}
}
