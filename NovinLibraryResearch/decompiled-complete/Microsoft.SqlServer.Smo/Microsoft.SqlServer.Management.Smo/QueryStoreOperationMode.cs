using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(QueryStoreOperationModeConverter))]
public enum QueryStoreOperationMode
{
	[LocDisplayName("Off")]
	[TsqlSyntaxString("OFF")]
	Off,
	[LocDisplayName("ReadOnly")]
	[TsqlSyntaxString("READ_ONLY")]
	ReadOnly,
	[LocDisplayName("ReadWrite")]
	[TsqlSyntaxString("READ_WRITE")]
	ReadWrite
}
