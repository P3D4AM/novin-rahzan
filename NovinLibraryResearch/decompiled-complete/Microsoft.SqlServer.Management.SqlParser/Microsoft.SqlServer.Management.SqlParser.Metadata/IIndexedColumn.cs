namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Duplicate("Name", "ReferencedColumn.Name")]
public interface IIndexedColumn : IMetadataObject
{
	[ReadOnly]
	[Reference]
	IColumn ReferencedColumn { get; }

	[ReadWrite]
	SortOrder SortOrder { get; }

	[ReadWrite]
	bool IsIncluded { get; }
}
