using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(AvailabilityReplicaSeedingModeConverter))]
public enum AvailabilityReplicaSeedingMode
{
	[TsqlSyntaxString("AUTOMATIC")]
	[LocDisplayName("seedingModeAutomatic")]
	Automatic,
	[TsqlSyntaxString("MANUAL")]
	[LocDisplayName("seedingModeManual")]
	Manual
}
