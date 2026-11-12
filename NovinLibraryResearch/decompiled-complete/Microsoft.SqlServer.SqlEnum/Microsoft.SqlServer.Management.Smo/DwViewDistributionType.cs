using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(DwViewDistributionConverter))]
public enum DwViewDistributionType
{
	Undefined = 0,
	[TsqlSyntaxString("HASH")]
	Hash = 2,
	[TsqlSyntaxString("ROUND_ROBIN")]
	RoundRobin = 4
}
