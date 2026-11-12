namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("IsOutput", true)]
[Constant("IsReadOnly", false)]
[Constant("DefaultValue", null)]
public interface ICursorParameter : ICursorVariable, ILocalVariable, IMetadataObject, IParameter
{
}
