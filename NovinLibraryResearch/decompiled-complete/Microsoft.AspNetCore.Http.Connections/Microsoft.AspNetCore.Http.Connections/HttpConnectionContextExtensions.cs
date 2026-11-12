using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace Microsoft.AspNetCore.Http.Connections;

public static class HttpConnectionContextExtensions
{
	public static HttpContext GetHttpContext(this ConnectionContext connection)
	{
		return connection.Features.Get<IHttpContextFeature>()?.HttpContext;
	}
}
