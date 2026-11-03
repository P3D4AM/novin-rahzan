using System;
using System.Collections;
using System.Collections.Generic;

namespace PacketParser.Assistance.Net;

public class TcpListenerManager<T> : IDictionary<int, T>, ICollection<KeyValuePair<int, T>>, IEnumerable<KeyValuePair<int, T>>, IEnumerable, IDisposable where T : class, IDisposable
{
	private Dictionary<int, T> listeners = new Dictionary<int, T>();

	private bool isDisposed;

	public T this[int port]
	{
		get
		{
			return listeners[port];
		}
		set
		{
			if (isDisposed)
			{
				throw new InvalidOperationException("You can not add to a disposed T");
			}
			if (listeners.ContainsKey(port))
			{
				if (listeners[port] != value)
				{
					throw new InvalidOperationException("The connection is already in cache");
				}
			}
			else
			{
				listeners[port] = value;
			}
		}
	}

	public ICollection<int> Keys => listeners.Keys;

	public ICollection<T> Values => listeners.Values;

	public int Count => listeners.Count;

	public bool IsReadOnly => false;

	public void Add(int key, T value)
	{
		if (isDisposed)
		{
			throw new InvalidOperationException("You can not add to a disposed T");
		}
		listeners[key] = value;
	}

	public void Add(KeyValuePair<int, T> item)
	{
		if (isDisposed)
		{
			throw new InvalidOperationException("You can not add to a disposed T");
		}
		listeners.Add(item.Key, item.Value);
	}

	public void Clear()
	{
		foreach (KeyValuePair<int, T> listener in listeners)
		{
			listener.Value.Dispose();
		}
		listeners.Clear();
	}

	public bool Contains(KeyValuePair<int, T> item)
	{
		return listeners.ContainsKey(item.Key);
	}

	public bool ContainsKey(int port)
	{
		return listeners.ContainsKey(port);
	}

	public void CopyTo(KeyValuePair<int, T>[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
	{
		return listeners.GetEnumerator();
	}

	public bool Remove(int port)
	{
		return listeners.Remove(port);
	}

	public bool Remove(KeyValuePair<int, T> item)
	{
		return listeners.Remove(item.Key);
	}

	public bool TryGetValue(int port, out T value)
	{
		return listeners.TryGetValue(port, out value);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Dispose()
	{
		isDisposed = true;
		foreach (KeyValuePair<int, T> listener in listeners)
		{
			listener.Value.Dispose();
		}
	}
}
