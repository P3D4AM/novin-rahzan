using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AvailabilityGroupNotJoinedOnReplicaException : HadrTaskBaseException
{
	public AvailabilityGroupNotJoinedOnReplicaException(string availabilityGroupName, string replicaServerName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AvailabilityGroupNotJoined, availabilityGroupName, replicaServerName))
	{
	}
}
