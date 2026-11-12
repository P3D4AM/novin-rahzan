namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IScalar : IMetadataObject
{
	[Abstract]
	ScalarType ScalarType { get; }

	[ReadWrite]
	[Reference]
	IScalarDataType DataType { get; }

	[ReadWrite]
	bool Nullable { get; }
}
