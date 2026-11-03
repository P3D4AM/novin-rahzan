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

public class TimingCache<Key, T> : INpDisposable, IDisposable where T : class
{
	private string _contextGuid;

	private IItemAsyncMapper<T, T> _defaultMapping;

	private Task _CollectEntity;

	private ConcurrentDictionary<Key, T> entities = new ConcurrentDictionary<Key, T>();

	private ConcurrentDictionary<Key, DateTime> timeDic = new ConcurrentDictionary<Key, DateTime>();

	private bool isRunning;

	private bool isNewable;

	private readonly SemaphoreSlim sem = new SemaphoreSlim(1, 1);

	private Dictionary<Key, SemaphoreSlim> _lock;

	public int CachingTime { get; set; }

	public bool IsDisposed { get; private set; }

	public event Func<object, List<Key>, Task> OnCollect;

	public event Func<object, List<T>, Task> OnCollectValues;

	public TimingCache(string contextGuid, IItemAsyncMapper<T, T> defaultMapping, int CachingTimeSecends)
	{
		int num = 0;
		try
		{
			_contextGuid = contextGuid;
			num = 2;
			_defaultMapping = defaultMapping;
			_lock = new Dictionary<Key, SemaphoreSlim>();
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
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\TimingCache.cs", 58);
		}
	}

	public int Count()
	{
		return entities.Count;
	}

	public async Task UpdateTimeAsync(Key key, T item, string tranName = "")
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		int line = 0;
		try
		{
			line = 1;
			if (!timeDic.TryGetValue(key, out var value))
			{
				line = 2;
				timeDic.TryAdd(key, DateTime.Now);
			}
			else
			{
				line = 3;
				timeDic.TryUpdate(key, DateTime.Now, value);
			}
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
							await _defaultMapping.MapAsync(_contextGuid, item, cachedItem, tranName);
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
				await sem.WaitAsync();
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
					sem.Release();
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
			WebErrorLog.Instance.StartLog(ex2, string.Format("Line:{0} item:{1}", line, item?.ToString() ?? "Null"), "UpdateTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\TimingCache.cs", 140);
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
			WebErrorLog.Instance.StartLog(ex, "", "CollectEntityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\TimingCache.cs", 169);
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
			WebErrorLog.Instance.StartLog(ex, "", "Clear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\TimingCache.cs", 203);
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

	public void ClearAll(bool fireCollectEvent = true)
	{
		using (new JaegerMonitor("ClearAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\TimingCache.cs"))
		{
			try
			{
				if (fireCollectEvent && this.OnCollect != null)
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
				WebErrorLog.Instance.StartLog(ex, "", "ClearAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\TimingCache.cs", 240);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Caching\\TimingCache.cs", 262);
		}
	}

	public T FirstOrDefault(Func<T, bool> value)
	{
		T val = entities.Values.FirstOrDefault(value);
		if (val != null)
		{
			foreach (KeyValuePair<Key, T> entity in entities)
			{
				if (entity.Value == val)
				{
					timeDic.TryUpdate(entity.Key, DateTime.Now, DateTime.Now);
					break;
				}
			}
		}
		return val;
	}

	public T First(Func<T, bool> value)
	{
		T val = entities.Values.First(value);
		if (val != null)
		{
			foreach (KeyValuePair<Key, T> entity in entities)
			{
				if (entity.Value == val)
				{
					timeDic.TryUpdate(entity.Key, DateTime.Now, DateTime.Now);
					break;
				}
			}
		}
		return val;
	}

	public T SingleOrDefault(Func<T, bool> value)
	{
		T val = entities.Values.SingleOrDefault(value);
		if (val != null)
		{
			foreach (KeyValuePair<Key, T> entity in entities)
			{
				if (entity.Value == val)
				{
					timeDic.TryUpdate(entity.Key, DateTime.Now, DateTime.Now);
					break;
				}
			}
		}
		return val;
	}

	public T Single(Func<T, bool> value)
	{
		T val = entities.Values.Single(value);
		if (val != null)
		{
			foreach (KeyValuePair<Key, T> entity in entities)
			{
				if (entity.Value == val)
				{
					timeDic.TryUpdate(entity.Key, DateTime.Now, DateTime.Now);
					break;
				}
			}
		}
		return val;
	}
}
