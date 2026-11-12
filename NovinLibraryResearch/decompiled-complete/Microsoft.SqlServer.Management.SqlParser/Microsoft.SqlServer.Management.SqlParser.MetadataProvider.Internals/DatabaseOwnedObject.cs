using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class DatabaseOwnedObject : DatabaseObject<IDatabase>, IMutableDatabaseOwnedObject, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseObject, IMutableMetadataObject
{
	public IDatabase Database => m_parent;

	protected DatabaseOwnedObject(IDatabase database, string name)
		: base(database, name)
	{
	}

	protected DatabaseOwnedObject(IDatabaseOwnedObject databaseOwnedObject)
		: base(databaseOwnedObject.Database, (IDatabaseObject)databaseOwnedObject)
	{
	}

	public sealed override T Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		return Accept(visitor);
	}

	public abstract T Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor);
}
