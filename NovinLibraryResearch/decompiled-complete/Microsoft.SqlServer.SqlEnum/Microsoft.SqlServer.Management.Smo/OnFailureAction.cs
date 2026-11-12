using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(AuditOnFailureActionConverter))]
public enum OnFailureAction
{
	[LocDisplayName("OnFailureActionContinue")]
	[TsqlSyntaxString("CONTINUE")]
	Continue,
	[LocDisplayName("OnFailureActionShutdown")]
	[TsqlSyntaxString("SHUTDOWN")]
	Shutdown,
	[LocDisplayName("OnFailureActionFail")]
	[TsqlSyntaxString("FAIL_OPERATION")]
	FailOperation
}
