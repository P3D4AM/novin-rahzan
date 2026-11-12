using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Common;

[CommonLocalizedPropertyResources("Microsoft.SqlServer.Management.Common.StringConnectionInfo")]
[TypeConverter(typeof(CommonLocalizableEnumConverter))]
public enum DatabaseEngineType
{
	Unknown,
	[CommonDisplayNameKey("Standalone")]
	Standalone,
	[CommonDisplayNameKey("SqlAzureDatabase")]
	SqlAzureDatabase
}
