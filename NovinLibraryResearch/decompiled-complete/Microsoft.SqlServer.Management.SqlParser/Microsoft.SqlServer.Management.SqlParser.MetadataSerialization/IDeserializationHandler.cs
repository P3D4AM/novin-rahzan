using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface IDeserializationHandler
{
	IMetadataObject OnCreateEvent(IMetadataObject obj);
}
