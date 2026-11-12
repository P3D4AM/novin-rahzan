using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IAsymmetricKeyFactory
{
	IMutableAsymmetricKey CreateAsymmetricKey(IDatabase database, string name);
}
