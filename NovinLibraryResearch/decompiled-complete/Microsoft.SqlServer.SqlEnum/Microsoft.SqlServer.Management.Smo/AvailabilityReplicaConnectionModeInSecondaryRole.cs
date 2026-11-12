using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(AvailabilityReplicaConnectionModeInSecondaryRoleConverter))]
public enum AvailabilityReplicaConnectionModeInSecondaryRole
{
	[LocDisplayName("cmsrNoConnections")]
	[TsqlSyntaxString("NO")]
	AllowNoConnections,
	[LocDisplayName("cmsrReadIntentConnectionsOnly")]
	[TsqlSyntaxString("READ_ONLY")]
	AllowReadIntentConnectionsOnly,
	[LocDisplayName("cmsrAllConnections")]
	[TsqlSyntaxString("ALL")]
	AllowAllConnections,
	[Browsable(false)]
	[LocDisplayName("Unknown")]
	Unknown
}
