using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(AvailabilityReplicaFailoverModeConverter))]
public enum AvailabilityReplicaFailoverMode
{
	[LocDisplayName("arfmAutomatic")]
	[TsqlSyntaxString("AUTOMATIC")]
	Automatic,
	[LocDisplayName("arfmManual")]
	[TsqlSyntaxString("MANUAL")]
	Manual,
	[LocDisplayName("arfmExternal")]
	[TsqlSyntaxString("EXTERNAL")]
	External,
	[Browsable(false)]
	[LocDisplayName("Unknown")]
	Unknown
}
