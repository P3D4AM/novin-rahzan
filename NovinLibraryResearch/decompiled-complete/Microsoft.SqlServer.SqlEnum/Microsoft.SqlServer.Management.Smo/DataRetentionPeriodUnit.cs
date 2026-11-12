using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(DataRetentionPeriodUnitTypeConverter))]
public enum DataRetentionPeriodUnit
{
	[TsqlSyntaxString("INFINITE")]
	[LocDisplayName("Infinite")]
	Infinite = -1,
	[TsqlSyntaxString("DAY")]
	[LocDisplayName("Day")]
	Day = 3,
	[TsqlSyntaxString("WEEK")]
	[LocDisplayName("Week")]
	Week = 4,
	[TsqlSyntaxString("MONTH")]
	[LocDisplayName("Month")]
	Month = 5,
	[TsqlSyntaxString("YEAR")]
	[LocDisplayName("Year")]
	Year = 6
}
