namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IUser : IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadOnly]
	UserType UserType { get; }

	[ReadWrite]
	[Reference]
	IAsymmetricKey AsymmetricKey { get; }

	[ReadWrite]
	[Reference]
	ICertificate Certificate { get; }

	[ReadWrite]
	[Reference]
	ILogin Login { get; }

	[ReadWrite]
	string Password { get; }

	[ReadWrite]
	[Reference]
	ISchema DefaultSchema { get; }
}
