using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class HadrTaskBaseException : Exception
{
	public HadrTaskBaseException(string message)
		: base(message)
	{
	}

	public HadrTaskBaseException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
