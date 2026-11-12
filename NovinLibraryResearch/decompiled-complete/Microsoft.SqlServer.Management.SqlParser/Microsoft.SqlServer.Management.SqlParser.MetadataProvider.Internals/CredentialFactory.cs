using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class CredentialFactory : ObjectFactoryBase<CredentialFactory>, ICredentialFactory
{
	public IMutableCredential CreateCredential(IServer server, string name)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Credential(server, name);
	}
}
