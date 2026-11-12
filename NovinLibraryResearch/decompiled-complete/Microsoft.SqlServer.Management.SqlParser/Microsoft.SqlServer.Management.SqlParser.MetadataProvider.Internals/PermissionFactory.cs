using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class PermissionFactory : ObjectFactoryBase<PermissionFactory>, IPermissionFactory
{
	public IDatabasePermission CreateDatabasePermission(IDatabasePrincipal databasePrincipal, IMetadataObject targetObject, DatabasePermissionType permissionType, IDatabasePrincipal grantor)
	{
		if (databasePrincipal == null)
		{
			throw new ArgumentNullException("databasePrincipal");
		}
		if (targetObject == null)
		{
			throw new ArgumentNullException("targetObject");
		}
		return new DatabasePermission(databasePrincipal, targetObject, permissionType, grantor);
	}
}
