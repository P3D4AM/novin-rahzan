using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class CredentialCollection : DualTypeSortedListCollection<ICredential, IDatabaseObject>
{
	public CredentialCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public CredentialCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
