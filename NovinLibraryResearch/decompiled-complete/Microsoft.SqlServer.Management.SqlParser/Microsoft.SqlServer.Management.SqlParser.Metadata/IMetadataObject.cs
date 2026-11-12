namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IMetadataObject
{
	[Abstract]
	string Name { get; }

	T Accept<T>(IMetadataObjectVisitor<T> visitor);
}
