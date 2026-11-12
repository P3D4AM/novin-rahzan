using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DatabaseCollection : DualTypeSortedListCollection<IDatabase, IDatabaseObject>
{
	public DatabaseCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public DatabaseCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
