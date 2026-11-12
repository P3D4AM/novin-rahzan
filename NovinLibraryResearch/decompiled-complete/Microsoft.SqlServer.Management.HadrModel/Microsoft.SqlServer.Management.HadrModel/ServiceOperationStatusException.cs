using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class ServiceOperationStatusException : HadrValidationBaseException
{
	public ServiceOperationStatusException(string message)
		: base(message)
	{
	}

	public ServiceOperationStatusException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
