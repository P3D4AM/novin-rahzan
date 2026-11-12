namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableApplicationRole : IApplicationRole, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabasePrincipal, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new ISchema DefaultSchema { get; set; }
}
