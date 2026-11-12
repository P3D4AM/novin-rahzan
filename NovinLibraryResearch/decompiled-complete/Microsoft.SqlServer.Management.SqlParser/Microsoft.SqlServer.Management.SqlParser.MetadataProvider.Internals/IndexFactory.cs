using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class IndexFactory : ObjectFactoryBase<IndexFactory>, IIndexFactory
{
	public IMutableIndexedColumn CreateIndexedColumn(IColumn referencedColumn)
	{
		if (referencedColumn == null)
		{
			throw new ArgumentNullException("referencedColumn");
		}
		return new IndexedColumn(referencedColumn);
	}

	public IMutableRelationalIndex CreateRelationalIndex(ITabular parent, string name, CollationInfo collationInfo)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (collationInfo == null)
		{
			throw new ArgumentNullException("collationInfo");
		}
		return new RelationalIndex(parent, name, collationInfo);
	}
}
