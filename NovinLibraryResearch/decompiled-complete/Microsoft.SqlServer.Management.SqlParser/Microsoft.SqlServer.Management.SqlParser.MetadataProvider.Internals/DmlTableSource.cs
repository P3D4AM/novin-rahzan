using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DmlTableSource : VirtualTable
{
	public override TabularType TabularType => TabularType.DmlTableSource;

	public DmlTableSource(string name, CollationInfo collationInfo)
		: base(name, collationInfo)
	{
	}
}
