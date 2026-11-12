using System;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

public static class SignalRAppBuilderExtensions
{
	public static IApplicationBuilder UseSignalR(this IApplicationBuilder app, Action<HubRouteBuilder> configure)
	{
		if (app.ApplicationServices.GetService<SignalRMarkerService>() == null)
		{
			throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddSignalR' inside the call to 'ConfigureServices(...)' in the application startup code.");
		}
		app.UseConnections(delegate(ConnectionsRouteBuilder routes)
		{
			configure(new HubRouteBuilder(routes));
		});
		return app;
	}
}
