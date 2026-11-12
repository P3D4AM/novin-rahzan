using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.RegisteredServers;

public sealed class ServerGroupCollection : SfcCollatedDictionaryCollection<ServerGroup, ServerGroup.Key, ServerGroupParent>
{
	public ServerGroup this[string name] => base[new ServerGroup.Key(name)];

	internal bool SkipInitialSqlLoad
	{
		set
		{
			base.Initialized = true;
		}
	}

	public ServerGroupCollection(ServerGroupParent parent, IComparer<string> customComparer)
		: base(parent, customComparer)
	{
	}

	public bool Contains(string name)
	{
		return Contains(new ServerGroup.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return ServerGroup.GetObjectFactory();
	}
}
