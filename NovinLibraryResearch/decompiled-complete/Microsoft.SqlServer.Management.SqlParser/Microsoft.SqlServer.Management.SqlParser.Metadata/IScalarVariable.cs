namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("ScalarType", ScalarType.ScalarVariable)]
public interface IScalarVariable : IScalar, IMetadataObject, ILocalVariable
{
	[ReadOnly]
	[Reference]
	new IScalarDataType DataType { get; }
}
