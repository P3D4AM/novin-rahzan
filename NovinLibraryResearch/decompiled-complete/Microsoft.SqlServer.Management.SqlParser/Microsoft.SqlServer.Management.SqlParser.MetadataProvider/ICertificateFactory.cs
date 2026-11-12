using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ICertificateFactory
{
	IMutableCertificate CreateCertificate(IDatabase database, string name);
}
