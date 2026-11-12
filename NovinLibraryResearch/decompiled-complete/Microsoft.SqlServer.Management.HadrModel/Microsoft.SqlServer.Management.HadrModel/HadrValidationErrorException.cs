using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class HadrValidationErrorException : Exception
{
	public HadrValidationErrorException(string message)
		: base(message)
	{
	}

	public HadrValidationErrorException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
