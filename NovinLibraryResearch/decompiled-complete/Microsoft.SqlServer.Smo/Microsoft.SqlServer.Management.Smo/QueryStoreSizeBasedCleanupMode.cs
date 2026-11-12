using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(QueryStoreSizeBasedCleanupModeConverter))]
public enum QueryStoreSizeBasedCleanupMode
{
	[LocDisplayName("Off")]
	[TsqlSyntaxString("OFF")]
	Off,
	[LocDisplayName("Auto")]
	[TsqlSyntaxString("AUTO")]
	Auto
}
