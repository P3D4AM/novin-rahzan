using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Data.Engine;

public static class StiDataConnections
{
	private static object lockObject = new object();

	private static Dictionary<IStiAppConnection, List<object>> connections = new Dictionary<IStiAppConnection, List<object>>();

	public static bool IsConnectionActive(IStiAppConnection connection)
	{
		if (connection == null)
		{
			return false;
		}
		lock (lockObject)
		{
			return connections.ContainsKey(connection);
		}
	}

	public static void RegisterConnection(IStiAppConnection connection, List<object> items)
	{
		if (connection == null)
		{
			return;
		}
		items = items ?? new List<object>();
		lock (lockObject)
		{
			if (connections.ContainsKey(connection))
			{
				List<object> list = connections[connection];
				if (list != null)
				{
					list.AddRange(items);
				}
				else
				{
					connections[connection] = items;
				}
			}
			else
			{
				connections.Add(connection, items);
			}
		}
	}

	public static List<object> UnRegisterConnections(List<IStiAppConnection> connections)
	{
		return connections?.Where((IStiAppConnection c) => c != null).SelectMany(UnRegisterConnection).ToList();
	}

	public static List<object> UnRegisterConnection(IStiAppConnection connection)
	{
		lock (lockObject)
		{
			if (connection == null || !connections.ContainsKey(connection))
			{
				return new List<object>();
			}
			List<object> list = connections[connection];
			connections.Remove(connection);
			return list ?? new List<object>();
		}
	}
}
