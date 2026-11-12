using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class HadrValidationWarningException : Exception
{
	public HadrValidationWarningException(string message)
		: base(message)
	{
	}

	public HadrValidationWarningException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
