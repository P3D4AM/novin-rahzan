using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(WorkloadManagementImportanceConverter))]
public enum WorkloadManagementImportance
{
	[TsqlSyntaxString("LOW")]
	Low,
	[TsqlSyntaxString("BELOW_NORMAL")]
	BelowNormal,
	[TsqlSyntaxString("NORMAL")]
	Normal,
	[TsqlSyntaxString("ABOVE_NORMAL")]
	AboveNormal,
	[TsqlSyntaxString("HIGH")]
	High
}
