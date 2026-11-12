using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace Microsoft.AspNetCore.SignalR;

public static class GetHttpContextExtensions
{
	public static HttpContext GetHttpContext(this HubCallerContext connection)
	{
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		return connection.Features.Get<IHttpContextFeature>()?.HttpContext;
	}

	public static HttpContext GetHttpContext(this HubConnectionContext connection)
	{
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		return connection.Features.Get<IHttpContextFeature>()?.HttpContext;
	}
}
