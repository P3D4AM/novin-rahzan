using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.RegisteredServers;

public sealed class RegisteredServerCollection : SfcCollatedDictionaryCollection<RegisteredServer, RegisteredServer.Key, ServerGroup>
{
	public RegisteredServer this[string name] => base[new RegisteredServer.Key(name)];

	internal bool SkipInitialSqlLoad
	{
		set
		{
			base.Initialized = true;
		}
	}

	public RegisteredServerCollection(ServerGroup parent, IComparer<string> customComparer)
		: base(parent, customComparer)
	{
	}

	public bool Contains(string name)
	{
		return Contains(new RegisteredServer.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return RegisteredServer.GetObjectFactory();
	}
}
