using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ScalarVariableCollection : DualTypeSortedListCollection<IScalarVariable, ILocalVariable>
{
	public ScalarVariableCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public ScalarVariableCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
