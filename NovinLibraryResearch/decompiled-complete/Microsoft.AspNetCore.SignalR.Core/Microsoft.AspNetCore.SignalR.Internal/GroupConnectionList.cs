using System.Collections.Concurrent;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class GroupConnectionList : ConcurrentDictionary<string, HubConnectionContext>
{
	public override bool Equals(object obj)
	{
		if (obj is ConcurrentDictionary<string, HubConnectionContext> concurrentDictionary)
		{
			return concurrentDictionary.Count == base.Count;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
