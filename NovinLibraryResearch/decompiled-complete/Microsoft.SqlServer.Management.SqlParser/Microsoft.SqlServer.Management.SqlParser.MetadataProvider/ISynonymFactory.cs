using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ISynonymFactory
{
	IMutableSynonym CreateSynonym(ISchema schema, string name);
}
