using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface ISerializationFilter
{
	bool IsIncluded(IMetadataObject metadataObject);
}
