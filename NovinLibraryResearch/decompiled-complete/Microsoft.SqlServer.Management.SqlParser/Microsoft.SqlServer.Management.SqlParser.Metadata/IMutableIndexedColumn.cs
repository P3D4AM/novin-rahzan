namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableIndexedColumn : IIndexedColumn, IMetadataObject, IMutableMetadataObject
{
	new SortOrder SortOrder { get; set; }

	new bool IsIncluded { get; set; }
}
