using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal interface IProxyMetadataObject
{
	IMetadataObject UnderlyingObject { get; }
}
