using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DatabaseFactory : ObjectFactoryBase<DatabaseFactory>, IDatabaseFactory
{
	public IMutableDatabase Create(IServer server, string name, CollationInfo collationInfo)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (collationInfo == null)
		{
			throw new ArgumentNullException("collationInfo");
		}
		return new Database(server, name, collationInfo);
	}

	public IMutableDatabase Create(IDatabase otherDatabase)
	{
		if (otherDatabase == null)
		{
			throw new ArgumentNullException("otherDatabase");
		}
		return new Database(otherDatabase);
	}

	public IDatabase CreateEmptyDatabase(IServer server, string name, CollationInfo collationInfo, bool isSystemObject)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (collationInfo == null)
		{
			throw new ArgumentNullException("collationInfo");
		}
		return new EmptyDatabase(server, name, collationInfo, isSystemObject);
	}
}
