using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ILoginFactory
{
	IMutableLogin CreateAsymmetricKeyLogin(IServer server, string name);

	IMutableLogin CreateCertificateLogin(IServer server, string name);

	IMutableLogin CreateSqlLogin(IServer server, string name);

	IMutableLogin CreateWindowsLogin(IServer server, string name);

	IMutablePassword CreatePassword();
}
