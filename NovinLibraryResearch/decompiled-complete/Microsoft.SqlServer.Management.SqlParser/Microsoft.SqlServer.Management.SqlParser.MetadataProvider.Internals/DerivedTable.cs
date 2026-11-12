using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DerivedTable : VirtualTable
{
	public override TabularType TabularType => TabularType.DerivedTable;

	public DerivedTable(string name, CollationInfo collationInfo)
		: base(name, collationInfo)
	{
	}
}
