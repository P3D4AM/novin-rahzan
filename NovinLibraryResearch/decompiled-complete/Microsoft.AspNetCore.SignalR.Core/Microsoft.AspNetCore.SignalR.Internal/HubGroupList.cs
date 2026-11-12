using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class HubGroupList : IReadOnlyCollection<ConcurrentDictionary<string, HubConnectionContext>>, IEnumerable<ConcurrentDictionary<string, HubConnectionContext>>, IEnumerable
{
	private readonly ConcurrentDictionary<string, GroupConnectionList> _groups = new ConcurrentDictionary<string, GroupConnectionList>(StringComparer.Ordinal);

	private static readonly GroupConnectionList EmptyGroupConnectionList = new GroupConnectionList();

	public ConcurrentDictionary<string, HubConnectionContext> this[string groupName]
	{
		get
		{
			_groups.TryGetValue(groupName, out var value);
			return value;
		}
	}

	public int Count => _groups.Count;

	public void Add(HubConnectionContext connection, string groupName)
	{
		CreateOrUpdateGroupWithConnection(groupName, connection);
	}

	public void Remove(string connectionId, string groupName)
	{
		if (_groups.TryGetValue(groupName, out var value) && value.TryRemove(connectionId, out var _) && value.IsEmpty)
		{
			KeyValuePair<string, GroupConnectionList> item = new KeyValuePair<string, GroupConnectionList>(groupName, EmptyGroupConnectionList);
			((ICollection<KeyValuePair<string, GroupConnectionList>>)_groups).Remove(item);
		}
	}

	public void RemoveDisconnectedConnection(string connectionId)
	{
		foreach (string item in from x in _groups
			where x.Value.Keys.Contains(connectionId)
			select x.Key)
		{
			Remove(connectionId, item);
		}
	}

	public IEnumerator<ConcurrentDictionary<string, HubConnectionContext>> GetEnumerator()
	{
		return _groups.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void CreateOrUpdateGroupWithConnection(string groupName, HubConnectionContext connection)
	{
		_groups.AddOrUpdate(groupName, (string _) => AddConnectionToGroup(connection, new GroupConnectionList()), delegate(string key, GroupConnectionList oldCollection)
		{
			AddConnectionToGroup(connection, oldCollection);
			return oldCollection;
		});
	}

	private static GroupConnectionList AddConnectionToGroup(HubConnectionContext connection, GroupConnectionList group)
	{
		group.AddOrUpdate(connection.ConnectionId, connection, (string _, HubConnectionContext __) => connection);
		return group;
	}
}
