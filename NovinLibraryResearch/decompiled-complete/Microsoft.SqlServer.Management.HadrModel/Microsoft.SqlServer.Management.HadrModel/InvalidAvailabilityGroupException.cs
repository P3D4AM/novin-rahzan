using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class InvalidAvailabilityGroupException : HadrTaskBaseException
{
	public InvalidAvailabilityGroupException(string availabilityGroupName, string serverName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.InvalidAvailabilityGroupException, availabilityGroupName, serverName))
	{
	}
}
