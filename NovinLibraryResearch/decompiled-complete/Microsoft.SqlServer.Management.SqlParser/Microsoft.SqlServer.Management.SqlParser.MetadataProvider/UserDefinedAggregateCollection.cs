using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class UserDefinedAggregateCollection : SortedListCollection<IUserDefinedAggregate>
{
	public UserDefinedAggregateCollection(CollationInfo collationInfo)
		: this(32, collationInfo)
	{
	}

	public UserDefinedAggregateCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
