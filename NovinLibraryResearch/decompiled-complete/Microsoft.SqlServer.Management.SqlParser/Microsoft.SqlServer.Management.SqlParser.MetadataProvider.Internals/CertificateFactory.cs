using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class CertificateFactory : ObjectFactoryBase<CertificateFactory>, ICertificateFactory
{
	public IMutableCertificate CreateCertificate(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Certificate(database, name);
	}
}
