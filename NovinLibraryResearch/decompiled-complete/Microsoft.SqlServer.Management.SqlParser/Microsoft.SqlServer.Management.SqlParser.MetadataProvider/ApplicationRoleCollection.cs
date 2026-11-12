using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class ApplicationRoleCollection : DualTypeSortedListCollection<IApplicationRole, IDatabasePrincipal>
{
	public ApplicationRoleCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public ApplicationRoleCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
