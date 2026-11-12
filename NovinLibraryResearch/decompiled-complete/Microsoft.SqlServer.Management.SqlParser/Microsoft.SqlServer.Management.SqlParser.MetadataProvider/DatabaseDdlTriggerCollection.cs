using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DatabaseDdlTriggerCollection : SortedListCollection<IDatabaseDdlTrigger>
{
	public DatabaseDdlTriggerCollection(CollationInfo collationInfo)
		: this(4, collationInfo)
	{
	}

	public DatabaseDdlTriggerCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
