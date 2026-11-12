using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class TableValuedFunctionCollection : DualTypeSortedListCollection<ITableValuedFunction, ITabular>
{
	public TableValuedFunctionCollection(CollationInfo collationInfo)
		: this(32, collationInfo)
	{
	}

	public TableValuedFunctionCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}

	internal static DualTypeSortedListCollection<ITableValuedFunction, ITabular> CreateBuiltInCollection(ISchema schema)
	{
		TableValuedFunctionCollection tableValuedFunctionCollection = new TableValuedFunctionCollection(schema.Database.CollationInfo);
		TableValuedFunction.PopulateBuiltInCollection(schema, tableValuedFunctionCollection);
		return tableValuedFunctionCollection;
	}
}
