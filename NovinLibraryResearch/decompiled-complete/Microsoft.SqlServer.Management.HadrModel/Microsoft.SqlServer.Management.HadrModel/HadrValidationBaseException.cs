using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class HadrValidationBaseException : Exception
{
	public HadrValidationBaseException(string message)
		: base(message)
	{
	}

	public HadrValidationBaseException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
