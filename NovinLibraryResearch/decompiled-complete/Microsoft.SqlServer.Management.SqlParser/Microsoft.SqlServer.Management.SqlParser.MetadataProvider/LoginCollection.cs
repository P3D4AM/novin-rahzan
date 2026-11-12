using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class LoginCollection : DualTypeSortedListCollection<ILogin, IDatabaseObject>
{
	public LoginCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public LoginCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
