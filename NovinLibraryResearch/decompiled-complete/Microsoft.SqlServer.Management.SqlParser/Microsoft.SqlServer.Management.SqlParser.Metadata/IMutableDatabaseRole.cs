namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDatabaseRole : IDatabaseRole, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabasePrincipal, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new bool IsFixedRole { get; set; }

	new IDatabasePrincipal Owner { get; set; }
}
