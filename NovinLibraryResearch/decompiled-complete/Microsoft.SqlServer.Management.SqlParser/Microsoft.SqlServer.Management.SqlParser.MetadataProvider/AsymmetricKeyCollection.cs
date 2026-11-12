using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class AsymmetricKeyCollection : DualTypeSortedListCollection<IAsymmetricKey, IDatabaseObject>
{
	public AsymmetricKeyCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public AsymmetricKeyCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
