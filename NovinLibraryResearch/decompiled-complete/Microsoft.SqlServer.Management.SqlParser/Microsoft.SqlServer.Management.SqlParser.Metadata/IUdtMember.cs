namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[ReadOnly("Name")]
[Constant("ScalarType", ScalarType.UdtMember)]
public interface IUdtMember : IScalar, IMetadataObject
{
	[ReadOnly]
	[Reference]
	IClrDataType UserDefinedType { get; }

	[ReadOnly]
	bool IsStatic { get; }
}
