using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ICollationLookup
{
	IMetadataCollection<ICollation> Collations { get; }
}
