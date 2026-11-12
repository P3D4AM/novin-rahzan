using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class StoredProcedureCollection : DualTypeSortedListCollection<IStoredProcedure, ICallableModule>
{
	public StoredProcedureCollection(CollationInfo collationInfo)
		: this(32, collationInfo)
	{
	}

	public StoredProcedureCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
