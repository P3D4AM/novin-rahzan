namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[Duplicate("Parent", "Server")]
public interface IServerOwnedObject : IDatabaseObject, IMetadataObject
{
	[ReadOnly]
	[Parent]
	IServer Server { get; }

	T Accept<T>(IServerOwnedObjectVisitor<T> visitor);
}
