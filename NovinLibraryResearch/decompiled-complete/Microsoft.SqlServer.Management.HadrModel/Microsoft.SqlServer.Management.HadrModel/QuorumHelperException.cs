using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class QuorumHelperException : HadrTaskBaseException
{
	public QuorumHelperException(string ReplicaName, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.QuorumHelperException, ReplicaName), inner)
	{
	}

	public QuorumHelperException(string ReplicaName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.QuorumHelperException, ReplicaName))
	{
	}
}
