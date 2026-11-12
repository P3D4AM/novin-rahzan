using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IDatabaseFactory
{
	IMutableDatabase Create(IServer server, string name, CollationInfo collationInfo);

	IMutableDatabase Create(IDatabase otherDatabase);

	IDatabase CreateEmptyDatabase(IServer server, string name, CollationInfo collationInfo, bool isSystemObject);
}
