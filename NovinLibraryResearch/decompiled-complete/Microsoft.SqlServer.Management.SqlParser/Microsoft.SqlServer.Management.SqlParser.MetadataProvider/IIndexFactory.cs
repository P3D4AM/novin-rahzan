using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IIndexFactory
{
	IMutableIndexedColumn CreateIndexedColumn(IColumn referencedColumn);

	IMutableRelationalIndex CreateRelationalIndex(ITabular parent, string name, CollationInfo collationInfo);
}
