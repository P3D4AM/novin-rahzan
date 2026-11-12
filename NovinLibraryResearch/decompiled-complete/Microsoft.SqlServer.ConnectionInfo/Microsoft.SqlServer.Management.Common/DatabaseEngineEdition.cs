using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Common;

[CommonLocalizedPropertyResources("Microsoft.SqlServer.Management.Common.StringConnectionInfo")]
[TypeConverter(typeof(CommonLocalizableEnumConverter))]
public enum DatabaseEngineEdition
{
	Unknown = 0,
	[CommonDisplayNameKey("PersonalEdition")]
	Personal = 1,
	[CommonDisplayNameKey("StandardEdition")]
	Standard = 2,
	[CommonDisplayNameKey("EnterpriseEdition")]
	Enterprise = 3,
	[CommonDisplayNameKey("ExpressEdition")]
	Express = 4,
	[CommonDisplayNameKey("SqlAzureDatabaseEdition")]
	SqlDatabase = 5,
	[CommonDisplayNameKey("SqlDataWarehouseEdition")]
	SqlDataWarehouse = 6,
	[CommonDisplayNameKey("StretchEdition")]
	SqlStretchDatabase = 7,
	[CommonDisplayNameKey("SqlManagedInstanceEdition")]
	SqlManagedInstance = 8,
	[CommonDisplayNameKey("SqlDatabaseEdgeEdition")]
	SqlDatabaseEdge = 9,
	[CommonDisplayNameKey("SqlOnDemandEdition")]
	SqlOnDemand = 11
}
