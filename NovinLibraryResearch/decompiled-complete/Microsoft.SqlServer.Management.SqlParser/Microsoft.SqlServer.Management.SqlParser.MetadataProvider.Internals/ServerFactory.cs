using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ServerFactory : ObjectFactoryBase<ServerFactory>, IServerFactory
{
	public IMutableServer Create(string name, CollationInfo collationInfo)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (collationInfo == null)
		{
			throw new ArgumentNullException("collationInfo");
		}
		return new Server(name, collationInfo);
	}

	public IMutableServer Create(IServer otherServer)
	{
		if (otherServer == null)
		{
			throw new ArgumentNullException("server");
		}
		return new Server(otherServer);
	}
}
