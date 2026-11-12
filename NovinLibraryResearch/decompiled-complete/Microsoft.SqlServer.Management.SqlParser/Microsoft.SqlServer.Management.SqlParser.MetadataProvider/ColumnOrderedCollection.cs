using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ColumnOrderedCollection : OrderedCollection<IColumn>
{
	public ColumnOrderedCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public ColumnOrderedCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
