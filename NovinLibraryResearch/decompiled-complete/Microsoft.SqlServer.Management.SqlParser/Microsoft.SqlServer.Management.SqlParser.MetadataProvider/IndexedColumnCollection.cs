using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class IndexedColumnCollection : OrderedCollection<IIndexedColumn>
{
	public IndexedColumnCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public IndexedColumnCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
