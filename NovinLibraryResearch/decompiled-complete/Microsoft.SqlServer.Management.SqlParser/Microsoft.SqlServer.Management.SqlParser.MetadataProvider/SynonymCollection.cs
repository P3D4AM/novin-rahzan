using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class SynonymCollection : SortedListCollection<ISynonym>
{
	public SynonymCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public SynonymCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
