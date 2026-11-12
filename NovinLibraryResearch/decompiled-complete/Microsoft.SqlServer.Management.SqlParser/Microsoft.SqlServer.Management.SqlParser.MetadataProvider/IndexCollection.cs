using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class IndexCollection : SortedListCollection<IIndex>
{
	public IndexCollection(CollationInfo collationInfo)
		: this(4, collationInfo)
	{
	}

	public IndexCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
