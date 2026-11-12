using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class AsymmetricKeyFactory : ObjectFactoryBase<AsymmetricKeyFactory>, IAsymmetricKeyFactory
{
	public IMutableAsymmetricKey CreateAsymmetricKey(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new AsymmetricKey(database, name);
	}
}
