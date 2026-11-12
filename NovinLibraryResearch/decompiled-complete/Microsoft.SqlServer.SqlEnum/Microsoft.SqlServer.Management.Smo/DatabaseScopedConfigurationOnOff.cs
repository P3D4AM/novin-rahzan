using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(DatabaseScopedConfigurationOnOffConverter))]
public enum DatabaseScopedConfigurationOnOff
{
	[LocDisplayName("dbScopedConfigurationOff")]
	[TsqlSyntaxString("OFF")]
	Off,
	[LocDisplayName("dbScopedConfigurationOn")]
	[TsqlSyntaxString("ON")]
	On,
	[LocDisplayName("dbScopedConfigurationPrimary")]
	[TsqlSyntaxString("PRIMARY")]
	Primary
}
