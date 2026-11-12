using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(AvailabilityReplicaConnectionModeInPrimaryRoleConverter))]
public enum AvailabilityReplicaConnectionModeInPrimaryRole
{
	[LocDisplayName("cmprAllConnections")]
	[TsqlSyntaxString("ALL")]
	AllowAllConnections = 2,
	[LocDisplayName("cmprReadWriteConnections")]
	[TsqlSyntaxString("READ_WRITE")]
	AllowReadWriteConnections,
	[Browsable(false)]
	[LocDisplayName("Unknown")]
	Unknown
}
