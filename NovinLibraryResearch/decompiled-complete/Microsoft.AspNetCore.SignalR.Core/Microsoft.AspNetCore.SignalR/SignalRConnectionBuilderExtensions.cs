using System;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Internal;

namespace Microsoft.AspNetCore.SignalR;

public static class SignalRConnectionBuilderExtensions
{
	public static IConnectionBuilder UseHub<THub>(this IConnectionBuilder connectionBuilder) where THub : Hub
	{
		if (connectionBuilder.ApplicationServices.GetService(typeof(SignalRCoreMarkerService)) == null)
		{
			throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddSignalR' inside the call to 'ConfigureServices(...)' in the application startup code.");
		}
		return connectionBuilder.UseConnectionHandler<HubConnectionHandler<THub>>();
	}
}
