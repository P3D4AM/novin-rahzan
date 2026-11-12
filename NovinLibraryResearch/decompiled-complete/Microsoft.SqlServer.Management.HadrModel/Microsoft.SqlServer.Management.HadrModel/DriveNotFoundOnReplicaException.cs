using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class DriveNotFoundOnReplicaException : HadrValidationErrorException
{
	public DriveNotFoundOnReplicaException(char driveLetter, string replicaName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.DataBaseDiskSizeValidationException, replicaName, driveLetter))
	{
	}
}
