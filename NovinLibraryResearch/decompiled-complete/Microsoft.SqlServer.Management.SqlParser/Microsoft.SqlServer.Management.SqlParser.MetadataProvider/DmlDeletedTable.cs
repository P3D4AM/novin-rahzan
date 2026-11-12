using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal class DmlDeletedTable : DmlInsertedDeletedTable
{
	public DmlDeletedTable(ITabular targetTable)
		: base("deleted", targetTable)
	{
	}
}
