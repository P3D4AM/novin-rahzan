using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IRoleFactory
{
	IMutableApplicationRole CreateApplicationRole(IDatabase database, string name);

	IMutableDatabaseRole CreateDatabaseRole(IDatabase database, string name);
}
