using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UnpivotTable : VirtualTable
{
	public override TabularType TabularType => TabularType.UnpivotTable;

	public UnpivotTable(string name, CollationInfo collationInfo)
		: base(name, collationInfo)
	{
	}
}
