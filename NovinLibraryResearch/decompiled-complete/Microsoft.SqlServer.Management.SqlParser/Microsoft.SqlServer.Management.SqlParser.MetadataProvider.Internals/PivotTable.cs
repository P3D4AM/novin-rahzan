using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class PivotTable : VirtualTable
{
	public override TabularType TabularType => TabularType.PivotTable;

	public PivotTable(string name, CollationInfo collationInfo)
		: base(name, collationInfo)
	{
	}
}
