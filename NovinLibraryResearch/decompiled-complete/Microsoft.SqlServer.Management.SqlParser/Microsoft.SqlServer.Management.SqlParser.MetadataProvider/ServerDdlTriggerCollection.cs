using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ServerDdlTriggerCollection : SortedListCollection<IServerDdlTrigger>
{
	public ServerDdlTriggerCollection(CollationInfo collationInfo)
		: this(4, collationInfo)
	{
	}

	public ServerDdlTriggerCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
