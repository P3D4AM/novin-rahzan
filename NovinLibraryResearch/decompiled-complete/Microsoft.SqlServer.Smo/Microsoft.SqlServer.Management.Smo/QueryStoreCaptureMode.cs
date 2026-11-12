using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(QueryStoreCaptureModeConverter))]
public enum QueryStoreCaptureMode
{
	[LocDisplayName("All")]
	[TsqlSyntaxString("ALL")]
	All = 1,
	[LocDisplayName("Auto")]
	[TsqlSyntaxString("AUTO")]
	Auto,
	[LocDisplayName("None")]
	[TsqlSyntaxString("NONE")]
	None,
	[LocDisplayName("Custom")]
	[TsqlSyntaxString("CUSTOM")]
	Custom
}
