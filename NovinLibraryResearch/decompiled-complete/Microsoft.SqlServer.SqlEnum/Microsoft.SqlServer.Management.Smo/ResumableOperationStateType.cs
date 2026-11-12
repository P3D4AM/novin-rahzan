using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(ResumableOperationStateTypeConverter))]
public enum ResumableOperationStateType
{
	[LocDisplayName("ResumableOperationStateTypeRunning")]
	[TsqlSyntaxString("RUNNING")]
	Running,
	[LocDisplayName("ResumableOperationStateTypePaused")]
	[TsqlSyntaxString("PAUSED")]
	Paused,
	[LocDisplayName("ResumableOperationStateTypeNone")]
	[TsqlSyntaxString("NONE")]
	None
}
