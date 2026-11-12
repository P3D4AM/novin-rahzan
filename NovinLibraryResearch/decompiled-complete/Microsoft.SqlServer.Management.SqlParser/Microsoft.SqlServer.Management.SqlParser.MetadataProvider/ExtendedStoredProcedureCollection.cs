using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ExtendedStoredProcedureCollection : DualTypeSortedListCollection<IExtendedStoredProcedure, ICallableModule>
{
	public ExtendedStoredProcedureCollection(CollationInfo collationInfo)
		: this(32, collationInfo)
	{
	}

	public ExtendedStoredProcedureCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
