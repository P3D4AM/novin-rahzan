using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace PacketParser.Caching;

public class AsyncTimingCache<Key, T> : INpDisposable, IDisposable where T : class
{
	private string _contextGuid;

	private IItemAsyncMapper<T, T> _defaiultMapping;

	private Task _CollectEntity;

	private ConcurrentDictionary<Key, T> entities = new ConcurrentDictionary<Key, T>();

	private ConcurrentDictionary<Key, DateTime> timeDic = new ConcurrentDictionary<Key, DateTime>();

	private bool isRunning;

	private bool isNewable;

	private readonly SemaphoreSlim sem = new SemaphoreSlim(1, 1);

	public int CachingTime { get; set; }

	public bool IsDisposed { get; private set; }

	public event Func<object, List<Key>, Task> OnCollect;

	public event Func<object, List<T>, Task> OnCollectValues;

	public AsyncTimingCache(string contextGuid, IItemAsyncMapper<T, T> defaultMapping, int CachingTimeSecends)
	{
		int num = 0;
		try
		{
			_contextGuid = contextGuid;
			num = 2;
			_defaiultMapping = defaultMapping;
			num = 3;
			CachingTime = CachingTimeSecends;
			num = 4;
			if (typeof(T) == typeof(Guid) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(string) || typeof(T) == typeof(short) || typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(bool) || typeof(T) == typeof(byte))
			{
				isNewable = false;
			}
			else
			{
				isNewable = true;
			}
			if (defaultMapping == null)
			{
				throw new Exception($"defaultMapping is null!!//Line:{num}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\AsyncTimingCache.cs", 56);
		}
	}

	public int Count()
	{
		return entities.Count;
	}

	public async Task UpdateTimeAsync(Key key, T item, string tranName = "")
	{
		int line = 0;
		try
		{
			line = 1;
			timeDic.AddOrUpdate(key, DateTime.Now, (Key a, DateTime b) => DateTime.Now);
			line = 4;
			if (item != null && !item.Equals(null))
			{
				line = 5;
				entities.TryRemove(key, out var cachedItem);
				line = 6;
				if (cachedItem != null && !cachedItem.Equals(null))
				{
					line = 7;
					if (cachedItem != item)
					{
						line = 8;
						if (isNewable)
						{
							await _defaiultMapping.MapAsync(_contextGuid, item, cachedItem, tranName);
						}
						else
						{
							cachedItem = item;
						}
					}
					line = 10;
				}
				else
				{
					cachedItem = item;
				}
				line = 11;
				entities.TryAdd(key, cachedItem);
				line = 12;
				if (!isRunning)
				{
					bool flag = await sem.WaitAsync(10000);
					try
					{
						line = 13;
						if (!isRunning)
						{
							line = 14;
							isRunning = true;
							line = 15;
							_CollectEntity = Task.Run((Func<Task?>)CollectEntityAsync);
						}
						line = 17;
					}
					finally
					{
						if (flag)
						{
							sem.Release();
						}
					}
				}
				line = 20;
			}
			line = 21;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, string.Format("Line:{0} item:{1}", line, item?.ToString() ?? "Null"), "UpdateTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\AsyncTimingCache.cs", 129);
		}
	}

	private async Task CollectEntityAsync()
	{
		try
		{
			isRunning = true;
			while (true)
			{
				ConcurrentDictionary<Key, T> concurrentDictionary = entities;
				if (concurrentDictionary == null || concurrentDictionary.Count <= 0)
				{
					break;
				}
				ICollection<Key> keys = timeDic.Keys;
				List<Key> list = new List<Key>();
				foreach (Key item in keys)
				{
					if (timeDic.TryGetValue(item, out var value) && DateTime.Now.Subtract(value).TotalSeconds > (double)CachingTime)
					{
						list.Add(item);
					}
				}
				if (list != null && list.Count > 0)
				{
					Clear(list);
				}
				await Task.Delay(30000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CollectEntityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\AsyncTimingCache.cs", 156);
		}
		finally
		{
			isRunning = false;
		}
	}

	public bool TryRemove(Key key, out T item)
	{
		timeDic.TryRemove(key, out var _);
		return entities.TryRemove(key, out item);
	}

	public void Clear(IEnumerable<Key> items)
	{
		try
		{
			ConcurrentDictionary<Key, DateTime> concurrentDictionary = timeDic;
			if (concurrentDictionary == null || concurrentDictionary.Count <= 0)
			{
				return;
			}
			List<T> list = new List<T>();
			List<Key> list2 = new List<Key>();
			if (items != null && items.Count() > 0)
			{
				foreach (Key item in items)
				{
					if (item != null && timeDic.ContainsKey(item))
					{
						timeDic.TryRemove(item, out var _);
						entities.TryRemove(item, out var value2);
						list.Add(value2);
						list2.Add(item);
					}
				}
			}
			this.OnCollect?.Invoke(this, list2);
			this.OnCollectValues?.Invoke(this, list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Clear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\AsyncTimingCache.cs", 189);
		}
	}

	public bool TryGetValue(Key key, out T ret)
	{
		if (key == null)
		{
			ret = null;
			return false;
		}
		bool num = entities.TryGetValue(key, out ret);
		if (num)
		{
			timeDic.TryUpdate(key, DateTime.Now, DateTime.Now);
		}
		return num;
	}

	internal void Remove(Key item)
	{
		timeDic.TryRemove(item, out var _);
		entities.TryRemove(item, out var _);
	}

	public void ClearAll()
	{
		using (new JaegerMonitor("ClearAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\AsyncTimingCache.cs"))
		{
			try
			{
				if (this.OnCollect != null)
				{
					ConcurrentDictionary<Key, DateTime> concurrentDictionary = timeDic;
					if (concurrentDictionary != null && concurrentDictionary.Count > 0)
					{
						List<Key> arg = timeDic.Keys?.ToList();
						this.OnCollect(this, arg);
					}
				}
				timeDic.Clear();
				entities.Clear();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ClearAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\AsyncTimingCache.cs", 228);
			}
		}
	}

	public List<T> GetAll()
	{
		foreach (Key key in timeDic.Keys)
		{
			timeDic.AddOrUpdate(key, DateTime.Now, (Key key, DateTime value) => DateTime.Now);
		}
		return entities.Values?.ToList();
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				timeDic.Clear();
				entities.Clear();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\AsyncTimingCache.cs", 252);
		}
	}
}
