namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface ITabular : IMetadataObject
{
	[Abstract]
	TabularType TabularType { get; }

	[Abstract]
	IMetadataOrderedCollection<IColumn> Columns { get; }

	[Abstract]
	ITabular Unaliased { get; }
}
