using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class HadrTaskErrorException : Exception
{
	public HadrTaskErrorException(string message)
		: base(message)
	{
	}

	public HadrTaskErrorException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
