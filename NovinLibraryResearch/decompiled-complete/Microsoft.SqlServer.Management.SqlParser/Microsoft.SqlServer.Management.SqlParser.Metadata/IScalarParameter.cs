namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[ReadOnly("IsOutput")]
[Constant("IsReadOnly", false)]
[ReadOnly("DefaultValue")]
public interface IScalarParameter : IScalarVariable, IScalar, IMetadataObject, ILocalVariable, IParameter
{
}
