namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[Duplicate("Parent", "Database")]
public interface IDatabaseOwnedObject : IDatabaseObject, IMetadataObject
{
	[ReadOnly]
	[Parent]
	IDatabase Database { get; }

	T Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor);
}
