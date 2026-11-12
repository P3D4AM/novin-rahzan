using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IUserFactory
{
	IMutableUser CreateAsymmetricKeyUser(IDatabase database, string name);

	IMutableUser CreateCertificateUser(IDatabase database, string name);

	IMutableUser CreateNoLoginUser(IDatabase database, string name);

	IMutableUser CreateSqlLoginUser(IDatabase database, string name);

	IMutableUser CreateExternalProviderUser(IDatabase database, string name);
}
