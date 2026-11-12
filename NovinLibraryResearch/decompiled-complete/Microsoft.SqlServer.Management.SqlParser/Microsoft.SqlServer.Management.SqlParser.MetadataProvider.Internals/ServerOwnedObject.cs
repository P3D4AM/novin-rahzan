using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class ServerOwnedObject : DatabaseObject<IServer>, IMutableServerOwnedObject, IServerOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseObject, IMutableMetadataObject
{
	public IServer Server => m_parent;

	protected ServerOwnedObject(IServer server, string name)
		: base(server, name)
	{
	}

	protected ServerOwnedObject(IServerOwnedObject serverOwnedObject)
		: base(serverOwnedObject.Server, (IDatabaseObject)serverOwnedObject)
	{
	}

	public sealed override T Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		return Accept(visitor);
	}

	public abstract T Accept<T>(IServerOwnedObjectVisitor<T> visitor);
}
