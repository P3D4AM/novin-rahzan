namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[ReadOnly("Name")]
public interface IConstraint : IMetadataObject
{
	[ReadOnly]
	[Parent]
	ITabular Parent { get; }

	[ReadWrite]
	bool IsSystemNamed { get; }

	[Abstract]
	ConstraintType Type { get; }
}
