using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class DatabaseFileAlreadyExistsOnReplicaException : HadrValidationErrorException
{
	public DatabaseFileAlreadyExistsOnReplicaException(string replicaName, IEnumerable<string> existingFiles)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.ValidatingDatabaseFileExistingError, replicaName, string.Join(CultureInfo.CurrentUICulture.TextInfo.ListSeparator, existingFiles)))
	{
	}
}
