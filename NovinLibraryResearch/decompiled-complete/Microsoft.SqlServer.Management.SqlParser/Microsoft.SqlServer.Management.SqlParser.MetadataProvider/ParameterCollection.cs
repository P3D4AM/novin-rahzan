using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ParameterCollection : OrderedCollection<IParameter>
{
	public ParameterCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public ParameterCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
