using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Common;

public static class ConnectionEnumsHelpers
{
	public static IEnumerable<DatabaseEngineEdition> GetSupportedDatabaseEngineEditions(this DatabaseEngineType engineType)
	{
		switch (engineType)
		{
		case DatabaseEngineType.SqlAzureDatabase:
			yield return DatabaseEngineEdition.SqlDatabase;
			yield return DatabaseEngineEdition.SqlDataWarehouse;
			yield return DatabaseEngineEdition.SqlOnDemand;
			break;
		case DatabaseEngineType.Standalone:
			yield return DatabaseEngineEdition.Personal;
			yield return DatabaseEngineEdition.Enterprise;
			yield return DatabaseEngineEdition.Express;
			yield return DatabaseEngineEdition.Standard;
			yield return DatabaseEngineEdition.SqlStretchDatabase;
			yield return DatabaseEngineEdition.SqlManagedInstance;
			yield return DatabaseEngineEdition.SqlDatabaseEdge;
			break;
		}
	}
}
