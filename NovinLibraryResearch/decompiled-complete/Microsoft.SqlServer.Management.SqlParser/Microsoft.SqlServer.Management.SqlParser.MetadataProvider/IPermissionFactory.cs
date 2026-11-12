using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IPermissionFactory
{
	IDatabasePermission CreateDatabasePermission(IDatabasePrincipal databasePrincipal, IMetadataObject targetObject, DatabasePermissionType permissionType, IDatabasePrincipal grantor);
}
