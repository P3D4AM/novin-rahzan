using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class TriggerFactory : ObjectFactoryBase<TriggerFactory>, ITriggerFactory
{
	public IMutableDmlTrigger CreateDmlTrigger(ITableViewBase parent, string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new DmlTrigger(parent, name);
	}

	public IMutableDatabaseDdlTrigger CreateDatabaseDdlTrigger(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new DatabaseDdlTrigger(database, name);
	}

	public IMutableServerDdlTrigger CreateServerDdlTrigger(IServer server, string name)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new ServerDdlTrigger(server, name);
	}
}
