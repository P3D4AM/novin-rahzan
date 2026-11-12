using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing.Internal;

namespace Microsoft.AspNetCore.Routing;

public class RouteBuilder : IRouteBuilder
{
	public IApplicationBuilder ApplicationBuilder { get; }

	public IRouter DefaultHandler { get; set; }

	public IServiceProvider ServiceProvider { get; }

	public IList<IRouter> Routes { get; }

	public RouteBuilder(IApplicationBuilder applicationBuilder)
		: this(applicationBuilder, null)
	{
	}

	public RouteBuilder(IApplicationBuilder applicationBuilder, IRouter defaultHandler)
	{
		if (applicationBuilder == null)
		{
			throw new ArgumentNullException("applicationBuilder");
		}
		if (applicationBuilder.ApplicationServices.GetService(typeof(RoutingMarkerService)) == null)
		{
			throw new InvalidOperationException(Resources.FormatUnableToFindServices("IServiceCollection", "AddRouting", "ConfigureServices(...)"));
		}
		ApplicationBuilder = applicationBuilder;
		DefaultHandler = defaultHandler;
		ServiceProvider = applicationBuilder.ApplicationServices;
		Routes = new List<IRouter>();
	}

	public IRouter Build()
	{
		RouteCollection routeCollection = new RouteCollection();
		foreach (IRouter route in Routes)
		{
			routeCollection.Add(route);
		}
		return routeCollection;
	}
}
