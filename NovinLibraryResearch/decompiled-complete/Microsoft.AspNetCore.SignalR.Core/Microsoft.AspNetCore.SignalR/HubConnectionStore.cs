using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR;

public class HubConnectionStore
{
	public readonly struct Enumerator : IEnumerator<HubConnectionContext>, IEnumerator, IDisposable
	{
		private readonly IEnumerator<KeyValuePair<string, HubConnectionContext>> _enumerator;

		public HubConnectionContext Current => _enumerator.Current.Value;

		object IEnumerator.Current => Current;

		public Enumerator(HubConnectionStore hubConnectionList)
		{
			_enumerator = hubConnectionList._connections.GetEnumerator();
		}

		public void Dispose()
		{
			_enumerator.Dispose();
		}

		public bool MoveNext()
		{
			return _enumerator.MoveNext();
		}

		public void Reset()
		{
			_enumerator.Reset();
		}
	}

	private readonly ConcurrentDictionary<string, HubConnectionContext> _connections = new ConcurrentDictionary<string, HubConnectionContext>(StringComparer.Ordinal);

	public HubConnectionContext this[string connectionId]
	{
		get
		{
			_connections.TryGetValue(connectionId, out var value);
			return value;
		}
	}

	public int Count => _connections.Count;

	public void Add(HubConnectionContext connection)
	{
		_connections.TryAdd(connection.ConnectionId, connection);
	}

	public void Remove(HubConnectionContext connection)
	{
		_connections.TryRemove(connection.ConnectionId, out var _);
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}
}
