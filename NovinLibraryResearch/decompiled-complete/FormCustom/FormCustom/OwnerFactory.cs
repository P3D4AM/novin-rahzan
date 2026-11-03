using System.Collections.Concurrent;
using FormCustom.Cache;

namespace FormCustom;

public class OwnerFactory
{
	private ConcurrentDictionary<string, CacheBaseForm> _instances = new ConcurrentDictionary<string, CacheBaseForm>();

	public static OwnerFactory Instance { get; private set; } = new OwnerFactory();


	public void SetOwner(string contextGuid, CacheBaseForm owner)
	{
		_instances.TryRemove(contextGuid, out var _);
		_instances.TryAdd(contextGuid, owner);
	}

	public CacheBaseForm GetOwner(string contextGuid)
	{
		_instances.TryGetValue(contextGuid, out var value);
		return value;
	}
}
