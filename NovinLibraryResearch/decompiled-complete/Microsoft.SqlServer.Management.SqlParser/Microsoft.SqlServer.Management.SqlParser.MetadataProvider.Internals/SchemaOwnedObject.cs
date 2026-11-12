using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class SchemaOwnedObject : DatabaseObject<ISchema>, IMutableSchemaOwnedObject, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseObject, IMutableMetadataObject
{
	public CollationInfo CollationInfo => m_parent.Database.CollationInfo;

	public ISchema Schema => m_parent;

	protected SchemaOwnedObject(ISchema schema, string name)
		: base(schema, name)
	{
	}

	protected SchemaOwnedObject(ISchemaOwnedObject schemaOwnedObject)
		: base(schemaOwnedObject.Schema, (IDatabaseObject)schemaOwnedObject)
	{
	}

	public sealed override T Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		return Accept(visitor);
	}

	public abstract T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor);
}
