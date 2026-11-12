namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDatabasePrincipal : IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new IMutableMetadataCollection<IDatabasePermission> Permissions { get; }

	new IMutableMetadataCollection<IDatabaseRole> MemberOfRoles { get; }
}
