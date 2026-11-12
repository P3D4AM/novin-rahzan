namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableUser : IUser, IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabasePrincipal, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new IAsymmetricKey AsymmetricKey { get; set; }

	new ICertificate Certificate { get; set; }

	new ILogin Login { get; set; }

	new string Password { get; set; }

	new ISchema DefaultSchema { get; set; }
}
