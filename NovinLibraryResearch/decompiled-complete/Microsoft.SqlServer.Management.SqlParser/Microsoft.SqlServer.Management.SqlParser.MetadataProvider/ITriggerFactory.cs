using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ITriggerFactory
{
	IMutableDmlTrigger CreateDmlTrigger(ITableViewBase parent, string name);

	IMutableDatabaseDdlTrigger CreateDatabaseDdlTrigger(IDatabase database, string name);

	IMutableServerDdlTrigger CreateServerDdlTrigger(IServer server, string name);
}
