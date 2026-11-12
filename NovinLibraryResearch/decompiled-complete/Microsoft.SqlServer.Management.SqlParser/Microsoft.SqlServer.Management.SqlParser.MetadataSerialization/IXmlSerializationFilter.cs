using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface IXmlSerializationFilter
{
	bool IsIncluded(IMetadataObject metadataObject);
}
