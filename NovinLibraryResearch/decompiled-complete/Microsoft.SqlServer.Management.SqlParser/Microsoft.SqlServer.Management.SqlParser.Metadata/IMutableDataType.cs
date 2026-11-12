namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDataType : IDataType, IMetadataObject, IMutableMetadataObject
{
	new bool IsUnknown { get; set; }
}
