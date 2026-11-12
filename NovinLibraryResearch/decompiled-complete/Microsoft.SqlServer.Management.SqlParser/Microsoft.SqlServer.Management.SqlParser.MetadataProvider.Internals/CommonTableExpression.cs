using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class CommonTableExpression : VirtualTable
{
	public override TabularType TabularType => TabularType.CommonTableExpression;

	public CommonTableExpression(string name, CollationInfo collationInfo)
		: base(name, collationInfo)
	{
	}
}
