namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IUniqueConstraintBase : IConstraint, IMetadataObject
{
	[ReadOnly]
	[Reference]
	IRelationalIndex AssociatedIndex { get; }
}
