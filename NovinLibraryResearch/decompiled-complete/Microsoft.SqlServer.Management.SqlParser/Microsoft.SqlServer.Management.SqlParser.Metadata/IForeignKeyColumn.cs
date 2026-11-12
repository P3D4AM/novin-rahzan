namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Duplicate("Name", "ReferencingColumn.Name")]
public interface IForeignKeyColumn : IMetadataObject
{
	[ReadOnly]
	[Reference]
	IColumn ReferencingColumn { get; }

	[ReadOnly]
	[Reference]
	IColumn ReferencedColumn { get; }
}
