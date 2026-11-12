using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class TableVariableCollection : DualTypeSortedListCollection<ITableVariable, ILocalVariable>
{
	public TableVariableCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public TableVariableCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
