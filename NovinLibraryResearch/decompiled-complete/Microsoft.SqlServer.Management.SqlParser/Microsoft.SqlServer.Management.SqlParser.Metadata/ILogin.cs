namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface ILogin : IServerOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadOnly]
	LoginType LoginType { get; }

	[ReadWrite]
	[Reference]
	IAsymmetricKey AsymmetricKey { get; }

	[ReadWrite]
	[Reference]
	ICertificate Certificate { get; }

	[ReadWrite]
	[Reference]
	ICredential Credential { get; }

	[ReadWrite]
	[Reference]
	IDatabase DefaultDatabase { get; }

	[ReadWrite]
	string Language { get; }

	[ReadWrite]
	IPassword Password { get; }

	[ReadWrite]
	byte[] Sid { get; }
}
