namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableLogin : ILogin, IServerOwnedObject, IDatabaseObject, IMetadataObject, IMutableServerOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new IAsymmetricKey AsymmetricKey { get; set; }

	new ICertificate Certificate { get; set; }

	new ICredential Credential { get; set; }

	new IDatabase DefaultDatabase { get; set; }

	new string Language { get; set; }

	new IPassword Password { get; set; }

	new byte[] Sid { get; set; }
}
