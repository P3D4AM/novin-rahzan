using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public sealed class UserDefinedDataTypeCollection : DualTypeSortedListCollection<IUserDefinedDataType, IUserDefinedType>
{
	public UserDefinedDataTypeCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public UserDefinedDataTypeCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
