namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[Duplicate("Parent", "Schema")]
public interface ISchemaOwnedObject : IDatabaseObject, IMetadataObject
{
	[ReadOnly]
	[Parent]
	ISchema Schema { get; }

	T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor);
}
