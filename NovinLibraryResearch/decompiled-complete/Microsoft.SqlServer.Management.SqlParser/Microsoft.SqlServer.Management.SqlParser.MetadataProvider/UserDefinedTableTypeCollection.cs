using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public sealed class UserDefinedTableTypeCollection : DualTypeSortedListCollection<IUserDefinedTableType, IUserDefinedType>
{
	public UserDefinedTableTypeCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public UserDefinedTableTypeCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
