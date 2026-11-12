using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.AspNetCore.Http.Connections.Internal;

internal class ConnectionLogScope : IReadOnlyList<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, IReadOnlyCollection<KeyValuePair<string, object>>
{
	private string _cachedToString;

	public string ConnectionId { get; set; }

	public KeyValuePair<string, object> this[int index]
	{
		get
		{
			if (Count == 1 && index == 0)
			{
				return new KeyValuePair<string, object>("TransportConnectionId", ConnectionId);
			}
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public int Count
	{
		get
		{
			if (!string.IsNullOrEmpty(ConnectionId))
			{
				return 1;
			}
			return 0;
		}
	}

	public ConnectionLogScope(string connectionId)
	{
		ConnectionId = connectionId;
	}

	public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		int i = 0;
		while (i < Count)
		{
			yield return this[i];
			int num = i + 1;
			i = num;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public override string ToString()
	{
		if (_cachedToString == null && !string.IsNullOrEmpty(ConnectionId))
		{
			_cachedToString = string.Format(CultureInfo.InvariantCulture, "TransportConnectionId:{0}", ConnectionId);
		}
		return _cachedToString;
	}
}
