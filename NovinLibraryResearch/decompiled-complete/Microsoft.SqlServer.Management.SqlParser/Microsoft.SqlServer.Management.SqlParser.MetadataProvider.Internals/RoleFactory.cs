using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class RoleFactory : ObjectFactoryBase<RoleFactory>, IRoleFactory
{
	public IMutableApplicationRole CreateApplicationRole(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new ApplicationRole(database, name);
	}

	public IMutableDatabaseRole CreateDatabaseRole(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new DatabaseRole(database, name);
	}
}
