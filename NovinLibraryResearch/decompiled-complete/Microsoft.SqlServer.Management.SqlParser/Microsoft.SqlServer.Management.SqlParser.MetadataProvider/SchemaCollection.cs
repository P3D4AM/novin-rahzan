using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class SchemaCollection : DualTypeSortedListCollection<ISchema, IDatabaseObject>
{
	public SchemaCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public SchemaCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
