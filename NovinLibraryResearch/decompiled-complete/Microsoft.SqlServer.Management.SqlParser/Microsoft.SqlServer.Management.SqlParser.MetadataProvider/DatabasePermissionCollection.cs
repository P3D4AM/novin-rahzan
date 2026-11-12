using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DatabasePermissionCollection : OrderedCollection<IDatabasePermission>
{
	public DatabasePermissionCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public DatabasePermissionCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
