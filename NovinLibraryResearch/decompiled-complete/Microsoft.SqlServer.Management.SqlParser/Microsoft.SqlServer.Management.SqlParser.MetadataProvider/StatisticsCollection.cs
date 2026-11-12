using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class StatisticsCollection : SortedListCollection<IStatistics>
{
	public StatisticsCollection(CollationInfo collationInfo)
		: this(4, collationInfo)
	{
	}

	public StatisticsCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
