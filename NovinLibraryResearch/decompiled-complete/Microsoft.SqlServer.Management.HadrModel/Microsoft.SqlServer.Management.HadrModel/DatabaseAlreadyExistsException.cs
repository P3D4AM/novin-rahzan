using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class DatabaseAlreadyExistsException : HadrValidationErrorException
{
	public DatabaseAlreadyExistsException(string replicaName, IEnumerable<string> existingDatabases)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.DatabasesExistingOnReplica, replicaName, string.Join(",", existingDatabases)))
	{
	}
}
