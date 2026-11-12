using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ForeignKeyColumnCollection : OrderedCollection<IForeignKeyColumn>
{
	public ForeignKeyColumnCollection(CollationInfo collationInfo)
		: this(4, collationInfo)
	{
	}

	public ForeignKeyColumnCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
