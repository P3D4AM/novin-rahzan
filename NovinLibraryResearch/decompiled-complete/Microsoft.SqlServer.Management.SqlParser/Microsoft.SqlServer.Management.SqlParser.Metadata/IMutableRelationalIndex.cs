namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableRelationalIndex : IRelationalIndex, IIndex, IMetadataObject, IMutableIndex, IMutableMetadataObject
{
	new bool CompactLargeObjects { get; set; }

	new IFileGroup FileGroup { get; set; }

	new IFileGroup FileStreamFileGroup { get; set; }

	new IPartitionScheme FileStreamPartitionScheme { get; set; }

	new string FilterDefinition { get; set; }

	new IMutableMetadataOrderedCollection<IIndexedColumn> IndexedColumns { get; }

	new IUniqueConstraintBase IndexKey { get; set; }

	new bool IsClustered { get; set; }

	new bool IsSystemNamed { get; set; }

	new bool IsUnique { get; set; }

	new bool NoAutomaticRecomputation { get; set; }

	new IPartitionScheme PartitionScheme { get; set; }
}
