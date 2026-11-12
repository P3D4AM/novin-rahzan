using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class TabularCollection : SortedListCollection<ITabular>
{
	public TabularCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public TabularCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
