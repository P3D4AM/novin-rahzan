using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class CertificateCollection : DualTypeSortedListCollection<ICertificate, IDatabaseObject>
{
	public CertificateCollection(CollationInfo collationInfo)
		: this(8, collationInfo)
	{
	}

	public CertificateCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}
}
