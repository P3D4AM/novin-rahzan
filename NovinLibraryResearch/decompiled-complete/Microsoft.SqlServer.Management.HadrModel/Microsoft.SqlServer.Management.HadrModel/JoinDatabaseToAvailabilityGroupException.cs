using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public class JoinDatabaseToAvailabilityGroupException : HadrTaskBaseException
{
	public JoinDatabaseToAvailabilityGroupException(Exception inner)
		: base(Resource.JoinAvailabilityGroupError, inner)
	{
	}
}
