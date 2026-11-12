using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ScalarValuedFunctionCollection : TriTypeSortedListCollection<IScalarValuedFunction, ICallableModule, IScalarFunction>
{
	public ScalarValuedFunctionCollection(CollationInfo collationInfo)
		: this(32, collationInfo)
	{
	}

	public ScalarValuedFunctionCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
