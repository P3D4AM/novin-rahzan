using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(SecurityPredicateTypeConverter))]
public enum SecurityPredicateType
{
	[LocDisplayName("securityPredicateTypeFilter")]
	[TsqlSyntaxString("FILTER")]
	Filter,
	[LocDisplayName("securityPredicateTypeBlock")]
	[TsqlSyntaxString("BLOCK")]
	Block
}
