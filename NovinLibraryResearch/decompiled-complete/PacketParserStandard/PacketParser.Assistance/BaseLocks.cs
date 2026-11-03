using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace PacketParser.Assistance;

public class BaseLocks
{
	protected TimingCache<string, MapperItemContainer<LockItem>> _locks;

	public BaseLocks(int cachingTimeSeconds = 60)
	{
		_locks = new TimingCache<string, MapperItemContainer<LockItem>>(Guid.NewGuid().ToString(), new MapperItemContainer<LockItem>(), cachingTimeSeconds);
		_locks.OnCollectValues += _locks_OnCollect;
	}

	private Task _locks_OnCollect(object sender, List<MapperItemContainer<LockItem>> items)
	{
		try
		{
			if (items != null && items.Any())
			{
				foreach (MapperItemContainer<LockItem> item in items)
				{
					item?.Value?.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_locks_OnCollect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BaseLocks.cs", 31);
		}
		return Task.CompletedTask;
	}

	public virtual async Task<SemaphoreSlim> GetSemaphoreAsync(string guid, string tranName = "")
	{
		MapperItemContainer<LockItem> ret = null;
		try
		{
			if (_locks.TryGetValue(guid, out ret) && ret != null)
			{
				return ret.Value.Semaphore;
			}
			ret = new MapperItemContainer<LockItem>
			{
				Value = new LockItem()
			};
			await _locks.UpdateTimeAsync(guid, ret, tranName);
		}
		catch (Exception ex)
		{
			_locks.TryRemove(guid, out var _);
			if (!ex.Message.Contains("The semaphore has been disposed"))
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSemaphoreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BaseLocks.cs", 48);
			}
		}
		return ret?.Value?.Semaphore;
	}

	public virtual async Task<LockItem> GetItemAsync(string guid, string tranName, params CancellationToken[] tokens)
	{
		MapperItemContainer<LockItem> ret = null;
		try
		{
			if (_locks.TryGetValue(guid, out ret) && ret != null)
			{
				return ret.Value;
			}
			ret = new MapperItemContainer<LockItem>
			{
				Value = new LockItem(tokens)
			};
			await _locks.UpdateTimeAsync(guid, ret, tranName);
		}
		catch (Exception ex)
		{
			_locks.TryRemove(guid, out var _);
			if (!ex.Message.Contains("The semaphore has been disposed"))
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BaseLocks.cs", 66);
			}
		}
		return ret?.Value;
	}
}
