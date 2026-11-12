namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableTabular : ITabular, IMetadataObject, IMutableMetadataObject
{
	new IMutableMetadataOrderedCollection<IColumn> Columns { get; }
}
