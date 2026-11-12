using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public sealed class UserDefinedClrTypeCollection : DualTypeSortedListCollection<IUserDefinedClrType, IUserDefinedType>
{
	public UserDefinedClrTypeCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public UserDefinedClrTypeCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
