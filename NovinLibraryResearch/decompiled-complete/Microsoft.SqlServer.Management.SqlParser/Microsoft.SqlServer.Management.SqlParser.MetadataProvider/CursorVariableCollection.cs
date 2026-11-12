using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class CursorVariableCollection : DualTypeSortedListCollection<ICursorVariable, ILocalVariable>
{
	public CursorVariableCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public CursorVariableCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
