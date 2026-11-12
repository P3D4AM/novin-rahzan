using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FailoverValidationException : HadrTaskBaseException
{
	public FailoverValidationException(string replicaName, string replicaRole)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.FailoverValidationException, replicaName, replicaRole))
	{
	}

	public FailoverValidationException(string replicaName, string replicaRole, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.FailoverValidationException, replicaName, replicaRole), inner)
	{
	}
}
