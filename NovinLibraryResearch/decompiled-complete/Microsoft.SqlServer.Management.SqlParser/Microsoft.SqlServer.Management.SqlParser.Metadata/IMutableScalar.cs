namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableScalar : IScalar, IMetadataObject, IMutableMetadataObject
{
	new IScalarDataType DataType { get; set; }

	new bool Nullable { get; set; }
}
