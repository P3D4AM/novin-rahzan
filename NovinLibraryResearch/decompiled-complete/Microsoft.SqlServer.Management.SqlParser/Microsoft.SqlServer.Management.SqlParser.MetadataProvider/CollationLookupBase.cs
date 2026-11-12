using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class CollationLookupBase : ICollationLookup
{
	private readonly IMetadataCollection<ICollation> m_collations;

	public IMetadataCollection<ICollation> Collations => m_collations;

	protected CollationLookupBase()
	{
		m_collations = CollationInfo.Collations;
	}
}
