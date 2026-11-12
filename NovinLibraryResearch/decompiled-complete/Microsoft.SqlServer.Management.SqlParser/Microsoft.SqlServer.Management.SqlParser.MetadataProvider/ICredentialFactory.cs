using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ICredentialFactory
{
	IMutableCredential CreateCredential(IServer server, string name);
}
