using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ConstraintCollection : SortedListCollection<IConstraint>
{
	public ConstraintCollection(CollationInfo collationInfo)
		: this(4, collationInfo)
	{
	}

	public ConstraintCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
