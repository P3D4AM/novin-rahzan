using System;

namespace Microsoft.AspNetCore.Http.Connections.Client;

public class NoTransportSupportedException : Exception
{
	public NoTransportSupportedException(string message)
		: base(message)
	{
	}
}
