using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class CreateIndexKeyList
{
	public IEnumerable<SqlIndexedColumn> IndexedColumns { get; set; }

	public IEnumerable<SqlIdentifier> IncludedColumns { get; set; }

	public CreateIndexKeyList(IEnumerable<SqlIndexedColumn> indexedColumns, IEnumerable<SqlIdentifier> includedColumns)
	{
		IndexedColumns = indexedColumns;
		IncludedColumns = includedColumns;
	}
}
