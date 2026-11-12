using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class DatabaseFileLocationMissingOnReplicaException : HadrValidationErrorException
{
	public DatabaseFileLocationMissingOnReplicaException(string replicaName, IEnumerable<string> missingFolders)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.DatabaseFileLocationMissingOnReplicaException, replicaName, string.Join(CultureInfo.CurrentUICulture.TextInfo.ListSeparator, missingFolders)))
	{
	}
}
