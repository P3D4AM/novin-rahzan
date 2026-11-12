using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Routing;

public interface IRouteBuilder
{
	IApplicationBuilder ApplicationBuilder { get; }

	IRouter DefaultHandler { get; set; }

	IServiceProvider ServiceProvider { get; }

	IList<IRouter> Routes { get; }

	IRouter Build();
}
