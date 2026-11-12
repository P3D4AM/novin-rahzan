using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(SensitivityRankConverter))]
public enum SensitivityRank
{
	[LocDisplayName("Undefined")]
	Undefined = -1,
	[LocDisplayName("None")]
	[TsqlSyntaxString("NONE")]
	None = 0,
	[LocDisplayName("Low")]
	[TsqlSyntaxString("LOW")]
	Low = 10,
	[LocDisplayName("Medium")]
	[TsqlSyntaxString("MEDIUM")]
	Medium = 20,
	[LocDisplayName("High")]
	[TsqlSyntaxString("HIGH")]
	High = 30,
	[LocDisplayName("Critical")]
	[TsqlSyntaxString("CRITICAL")]
	Critical = 40
}
