using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(ExternalDataSourcePushdownOptionConverter))]
public enum ExternalDataSourcePushdownOption
{
	[TsqlSyntaxString("OFF")]
	Off,
	[TsqlSyntaxString("ON")]
	On
}
