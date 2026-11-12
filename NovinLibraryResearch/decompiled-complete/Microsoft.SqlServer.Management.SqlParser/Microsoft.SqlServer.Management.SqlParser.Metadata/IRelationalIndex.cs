namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("Type", IndexType.Relational)]
public interface IRelationalIndex : IIndex, IMetadataObject
{
	[ReadWrite]
	bool CompactLargeObjects { get; }

	[ReadWrite]
	[Child]
	IFileGroup FileGroup { get; }

	[ReadWrite]
	[Child]
	IFileGroup FileStreamFileGroup { get; }

	[ReadWrite]
	[Child]
	IPartitionScheme FileStreamPartitionScheme { get; }

	[ReadWrite]
	string FilterDefinition { get; }

	[ReadWrite]
	IMetadataOrderedCollection<IIndexedColumn> IndexedColumns { get; }

	[ReadWrite]
	[Reference]
	IUniqueConstraintBase IndexKey { get; }

	[ReadWrite]
	bool IsClustered { get; }

	[ReadWrite]
	bool IsSystemNamed { get; }

	[ReadWrite]
	bool IsUnique { get; }

	[ReadWrite]
	bool NoAutomaticRecomputation { get; }

	[ReadWrite]
	[Child]
	IPartitionScheme PartitionScheme { get; }
}
