using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using Server.Core.Assistance;

namespace Server.Core.Connection;

public class ConnectionDataAggregator : IDisposable
{
	private ConcurrentDictionary<ENSource, List<ClsConnectionData>> source2Connection = new ConcurrentDictionary<ENSource, List<ClsConnectionData>>();

	private ConcurrentDictionary<long, ClsConnectionData> Hash2Connection = new ConcurrentDictionary<long, ClsConnectionData>();

	public IEnumerable<ClsConnectionData> Connections => Hash2Connection.Values?.Where((ClsConnectionData e) => e != null && !e.IsDisposed)?.ToList();

	public void Add(ClsConnectionData connection, string guid)
	{
		int num = 0;
		List<ClsConnectionData> value = null;
		try
		{
			num = 1;
			num = 2;
			num = 3;
			source2Connection.TryGetValue(connection.Source, out value);
			num = (num = 400);
			if (value == null || !value.Any())
			{
				num = 41;
				value = new List<ClsConnectionData> { connection };
				num = 6;
				source2Connection.TryRemove(connection.Source, out var _);
				num = 7;
				source2Connection.TryAdd(connection.Source, value);
				num = 8;
			}
			else
			{
				num = 50;
				value.Add(connection);
			}
			num = 15;
			Hash2Connection.TryRemove(connection.HashId, out var _);
			num = 16;
			Hash2Connection.TryAdd(connection.HashId, connection);
			num = 17;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}//connectionsBySource is null:{value == null}", "Add", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionDataAggregator.cs", 58);
		}
	}

	public List<string> GetConnections(ENSource source)
	{
		List<string> list = new List<string>();
		source2Connection.TryGetValue(source, out var value);
		if (value != null)
		{
			foreach (ClsConnectionData item in value)
			{
				list.Add(item.Guid);
			}
		}
		return list;
	}

	internal ClsConnectionData getConnection(int HashId)
	{
		Hash2Connection.TryGetValue(HashId, out var value);
		return value;
	}

	public List<string> GetGuids()
	{
		try
		{
			List<string> list = new List<string>();
			if (Hash2Connection != null)
			{
				foreach (ClsConnectionData item in Hash2Connection.Values?.ToList())
				{
					if (item != null && !item.IsDisposed)
					{
						list.Add(item.Guid);
					}
				}
			}
			return list?.Where((string p) => p != null)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGuids", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionDataAggregator.cs", 90);
			return null;
		}
	}

	public List<string> GetGuids(ENSource Source)
	{
		source2Connection.TryGetValue(Source, out var value);
		if (value == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		foreach (ClsConnectionData item in value)
		{
			list.Add(item.Guid);
		}
		return list;
	}

	public List<string> GetGuids(int hashId)
	{
		return (from e in Connections
			where e.HashId == hashId
			select e.Guid)?.ToList();
	}

	internal bool IsOnline(long hashId)
	{
		try
		{
			Hash2Connection.TryGetValue(hashId, out var value);
			if (value == null)
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsOnline", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionDataAggregator.cs", 120);
			return false;
		}
	}

	public void Dispose()
	{
		try
		{
			if (Hash2Connection != null)
			{
				foreach (ClsConnectionData item in Hash2Connection.Values?.ToList())
				{
					item?.Dispose("ConnectionDataAggregator_Dispose");
				}
			}
			Hash2Connection = null;
			source2Connection = null;
			Hash2Connection = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionDataAggregator.cs", 136);
		}
	}

	internal async Task UnHandleConnection(ClsConnectionData connection, string reason)
	{
		using (new JaegerMonitor("UnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionDataAggregator.cs"))
		{
			try
			{
				List<ClsConnectionData> value = null;
				source2Connection?.TryRemove(connection.Source, out value);
				value?.Remove(connection);
				if (value != null && value.Count > 0)
				{
					source2Connection.TryAdd(connection.Source, value);
				}
				Hash2Connection.TryRemove(connection.HashId, out var value2);
				if (Initializer.Dal.ServerLog != null)
				{
					Task.Run(() => Initializer.Dal.ServerLog.InsertLogAsync(connection?.Ip ?? "", 0, 1, $"**UnHandleConnection**_connection //HasdhId:{connection?.HashId}//DepId:{connection?.DepId}//DepName:{connection?.DepName}//User:{connection?.Username}", default(CancellationToken), 10));
				}
				connection?.Dispose("ConnectionDataAggregator.UnHandleConnection_Reason:" + reason);
				if (connection == null)
				{
					Hash2Connection.TryRemove(connection.HashId, out value2);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionDataAggregator.cs", 161);
			}
		}
	}
}
