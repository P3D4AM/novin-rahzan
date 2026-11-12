using System;

namespace Microsoft.AspNetCore.Routing;

public class RouteCreationException : Exception
{
	public RouteCreationException(string message)
		: base(message)
	{
	}

	public RouteCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
