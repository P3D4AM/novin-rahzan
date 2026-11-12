using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DmlTriggerCollection : SortedListCollection<IDmlTrigger>
{
	public DmlTriggerCollection(CollationInfo collationInfo)
		: this(4, collationInfo)
	{
	}

	public DmlTriggerCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
