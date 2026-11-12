using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(AuditDestinationTypeConverter))]
public enum AuditDestinationType
{
	[LocDisplayName("FileDest")]
	[TsqlSyntaxString("FILE")]
	File = 0,
	[LocDisplayName("SecurityLogDest")]
	[TsqlSyntaxString("SECURITY_LOG")]
	SecurityLog = 1,
	[LocDisplayName("ApplicationLogDest")]
	[TsqlSyntaxString("APPLICATION_LOG")]
	ApplicationLog = 2,
	[LocDisplayName("UrlDest")]
	[TsqlSyntaxString("URL")]
	Url = 3,
	[LocDisplayName("ExternalMonitorDest")]
	[TsqlSyntaxString("EXTERNAL_MONITOR")]
	ExternalMonitor = 4,
	[LocDisplayName("UnknownDest")]
	[TsqlSyntaxString("UNKNOWN")]
	Unknown = 100
}
