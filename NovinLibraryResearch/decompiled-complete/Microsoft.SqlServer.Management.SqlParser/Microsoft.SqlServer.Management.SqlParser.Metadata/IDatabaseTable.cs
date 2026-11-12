namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[Constant("Unaliased", SpecialValue.This)]
[ReadWrite("Columns")]
[Child("Columns")]
public interface IDatabaseTable : ITabular, IMetadataObject
{
	[Abstract]
	CollationInfo CollationInfo { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IConstraint> Constraints { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IIndex> Indexes { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IStatistics> Statistics { get; }
}
