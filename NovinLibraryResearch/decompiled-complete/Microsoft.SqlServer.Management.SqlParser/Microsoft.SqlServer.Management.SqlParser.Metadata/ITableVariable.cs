namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("TabularType", TabularType.TableVariable)]
[Constant("Unaliased", SpecialValue.This)]
[Duplicate("Columns", "DataType.Columns")]
public interface ITableVariable : ITabular, IMetadataObject, ILocalVariable
{
	[ReadOnly]
	[Reference]
	new ITableDataType DataType { get; }
}
