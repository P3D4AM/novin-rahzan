using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Http.Connections;

public class ConnectionsRouteBuilder
{
	private readonly HttpConnectionDispatcher _dispatcher;

	private readonly RouteBuilder _routes;

	internal ConnectionsRouteBuilder(RouteBuilder routes, HttpConnectionDispatcher dispatcher)
	{
		_routes = routes;
		_dispatcher = dispatcher;
	}

	public void MapConnections(PathString path, Action<IConnectionBuilder> configure)
	{
		MapConnections(path, new HttpConnectionDispatcherOptions(), configure);
	}

	public void MapConnections(PathString path, HttpConnectionDispatcherOptions options, Action<IConnectionBuilder> configure)
	{
		ConnectionBuilder connectionBuilder = new ConnectionBuilder(_routes.ServiceProvider);
		configure(connectionBuilder);
		ConnectionDelegate socket = connectionBuilder.Build();
		_routes.MapRoute(path, (HttpContext c) => _dispatcher.ExecuteAsync(c, options, socket));
		_routes.MapRoute(path + "/negotiate", (HttpContext c) => _dispatcher.ExecuteNegotiateAsync(c, options));
	}

	public void MapConnectionHandler<TConnectionHandler>(PathString path) where TConnectionHandler : ConnectionHandler
	{
		MapConnectionHandler<TConnectionHandler>(path, null);
	}

	public void MapConnectionHandler<TConnectionHandler>(PathString path, Action<HttpConnectionDispatcherOptions> configureOptions) where TConnectionHandler : ConnectionHandler
	{
		IEnumerable<AuthorizeAttribute> customAttributes = typeof(TConnectionHandler).GetCustomAttributes<AuthorizeAttribute>(inherit: true);
		HttpConnectionDispatcherOptions httpConnectionDispatcherOptions = new HttpConnectionDispatcherOptions();
		foreach (AuthorizeAttribute item in customAttributes)
		{
			httpConnectionDispatcherOptions.AuthorizationData.Add(item);
		}
		configureOptions?.Invoke(httpConnectionDispatcherOptions);
		MapConnections(path, httpConnectionDispatcherOptions, delegate(IConnectionBuilder builder)
		{
			builder.UseConnectionHandler<TConnectionHandler>();
		});
	}
}
