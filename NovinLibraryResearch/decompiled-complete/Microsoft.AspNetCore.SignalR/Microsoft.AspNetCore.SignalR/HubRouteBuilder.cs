using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;

namespace Microsoft.AspNetCore.SignalR;

public class HubRouteBuilder
{
	private readonly ConnectionsRouteBuilder _routes;

	public HubRouteBuilder(ConnectionsRouteBuilder routes)
	{
		_routes = routes;
	}

	public void MapHub<THub>(PathString path) where THub : Hub
	{
		MapHub<THub>(path, null);
	}

	public void MapHub<THub>(PathString path, Action<HttpConnectionDispatcherOptions> configureOptions) where THub : Hub
	{
		IEnumerable<AuthorizeAttribute> customAttributes = typeof(THub).GetCustomAttributes<AuthorizeAttribute>(inherit: true);
		HttpConnectionDispatcherOptions httpConnectionDispatcherOptions = new HttpConnectionDispatcherOptions();
		foreach (AuthorizeAttribute item in customAttributes)
		{
			httpConnectionDispatcherOptions.AuthorizationData.Add(item);
		}
		configureOptions?.Invoke(httpConnectionDispatcherOptions);
		_routes.MapConnections(path, httpConnectionDispatcherOptions, delegate(IConnectionBuilder builder)
		{
			builder.UseHub<THub>();
		});
	}
}
