using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class UserCollection : DualTypeSortedListCollection<IUser, IDatabasePrincipal>
{
	public UserCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public UserCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
