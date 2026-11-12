namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("Parent", null)]
public interface IServer : IDatabaseObject, IMetadataObject
{
	[ReadOnly]
	CollationInfo CollationInfo { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IDatabase> Databases { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<ICredential> Credentials { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<ILogin> Logins { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IServerDdlTrigger> Triggers { get; }
}
