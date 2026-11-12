namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[ReadOnly("Name")]
public interface IDatabaseObject : IMetadataObject
{
	[Abstract]
	IDatabaseObject Parent { get; }

	[ReadWrite]
	bool IsSystemObject { get; }

	T Accept<T>(IDatabaseObjectVisitor<T> visitor);
}
