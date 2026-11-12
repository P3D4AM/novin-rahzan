using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class DatabasePrincipal : DatabaseOwnedObject, IMutableDatabasePrincipal, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private readonly DatabasePermissionCollection permissions;

	private readonly DatabaseRoleCollection memberOfRoles;

	public IMutableMetadataCollection<IDatabasePermission> Permissions => permissions;

	public IMutableMetadataCollection<IDatabaseRole> MemberOfRoles => memberOfRoles;

	IMetadataCollection<IDatabasePermission> IDatabasePrincipal.Permissions => permissions;

	IMetadataCollection<IDatabaseRole> IDatabasePrincipal.MemberOfRoles => memberOfRoles;

	public DatabasePrincipal(IDatabase database, string name)
		: base(database, name)
	{
		permissions = new DatabasePermissionCollection(database.CollationInfo);
		memberOfRoles = new DatabaseRoleCollection(database.CollationInfo);
	}
}
