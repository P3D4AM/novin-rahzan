using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DatabaseRoleCollection : DualTypeSortedListCollection<IDatabaseRole, IDatabasePrincipal>
{
	public DatabaseRoleCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public DatabaseRoleCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
