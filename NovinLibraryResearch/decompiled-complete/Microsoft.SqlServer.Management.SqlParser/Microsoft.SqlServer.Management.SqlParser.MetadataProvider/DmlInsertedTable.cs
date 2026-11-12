using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal class DmlInsertedTable : DmlInsertedDeletedTable
{
	public DmlInsertedTable(ITabular targetTable)
		: base("inserted", targetTable)
	{
	}
}
