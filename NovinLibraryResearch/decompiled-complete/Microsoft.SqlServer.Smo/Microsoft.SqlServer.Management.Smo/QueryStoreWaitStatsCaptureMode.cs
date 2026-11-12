using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(QueryStoreWaitStatsCaptureModeConverter))]
public enum QueryStoreWaitStatsCaptureMode
{
	[LocDisplayName("Off")]
	[TsqlSyntaxString("OFF")]
	Off,
	[LocDisplayName("On")]
	[TsqlSyntaxString("ON")]
	On
}
