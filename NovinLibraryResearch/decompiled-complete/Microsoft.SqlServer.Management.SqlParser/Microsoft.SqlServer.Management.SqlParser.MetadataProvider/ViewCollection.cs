using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ViewCollection : TriTypeSortedListCollection<IView, IDatabaseTable, ITabular>
{
	public ViewCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public ViewCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
