using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ColumnCollection : DualTypeSortedListCollection<IColumn, IScalar>
{
	public ColumnCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public ColumnCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
