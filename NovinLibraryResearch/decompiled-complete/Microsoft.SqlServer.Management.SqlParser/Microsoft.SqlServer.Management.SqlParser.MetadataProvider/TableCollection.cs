using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class TableCollection : TriTypeSortedListCollection<ITable, IDatabaseTable, ITabular>
{
	public TableCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public TableCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
