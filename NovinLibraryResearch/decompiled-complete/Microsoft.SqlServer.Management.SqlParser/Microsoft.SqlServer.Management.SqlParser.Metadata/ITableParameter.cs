namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("IsOutput", false)]
[Constant("IsReadOnly", true)]
[Constant("DefaultValue", null)]
public interface ITableParameter : ITableVariable, ITabular, IMetadataObject, ILocalVariable, IParameter
{
}
