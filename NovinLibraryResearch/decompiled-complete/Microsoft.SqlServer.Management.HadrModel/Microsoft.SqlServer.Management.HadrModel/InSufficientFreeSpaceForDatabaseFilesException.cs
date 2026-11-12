using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class InSufficientFreeSpaceForDatabaseFilesException : HadrValidationErrorException
{
	public InSufficientFreeSpaceForDatabaseFilesException(string replicaName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.InSufficientFreeSpaceForDataFiles, replicaName))
	{
	}
}
