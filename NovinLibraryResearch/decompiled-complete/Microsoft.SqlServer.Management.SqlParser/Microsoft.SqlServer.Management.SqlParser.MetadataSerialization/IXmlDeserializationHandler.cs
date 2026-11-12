using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface IXmlDeserializationHandler
{
	IMetadataObject OnCreatEvent(IMetadataObject obj);
}
