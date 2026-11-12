namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableScalarDataType : IScalarDataType, IDataType, IMetadataObject, IMutableDataType, IMutableMetadataObject
{
	new ISystemDataType BaseSystemDataType { get; set; }
}
