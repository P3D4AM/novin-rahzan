namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableServer : IServer, IDatabaseObject, IMetadataObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new IMutableMetadataCollection<IDatabase> Databases { get; }

	new IMutableMetadataCollection<ICredential> Credentials { get; }

	new IMutableMetadataCollection<ILogin> Logins { get; }

	new IMutableMetadataCollection<IServerDdlTrigger> Triggers { get; }
}
