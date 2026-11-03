using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace PacketParser;

public class CollectionWatcher<K, T> : IDisposable where T : class
{
	private ConcurrentDictionary<K, T> _dic;

	private readonly int _timeOutMiliSeconds;

	private readonly short _maxCollections;

	private bool TimeOutEnable;

	private SemaphoreSlim sem = new SemaphoreSlim(1, 1);

	private SemaphoreSlim semReady = new SemaphoreSlim(1, 1);

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	public event Func<object, List<T>, Task> DataReady;

	public CollectionWatcher(int TimeOutMiliSeconds, short MaxCollections, IItemAsyncMapper<T, T> defaultMapping)
	{
		try
		{
			_timeOutMiliSeconds = TimeOutMiliSeconds;
			_maxCollections = MaxCollections;
			Task.Run(async delegate
			{
				await TimeOutTask();
			});
			_dic = new ConcurrentDictionary<K, T>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CollectionWatcher.cs", 43);
		}
	}

	private async Task TimeOutTask()
	{
		while (Count() > 0)
		{
			try
			{
				TimeOutEnable = true;
				await Task.Delay(_timeOutMiliSeconds);
				await RaiseDataReadyAsync();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TimeOutTask", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CollectionWatcher.cs", 61);
			}
		}
		TimeOutEnable = false;
	}

	public async Task Add(K key, T item)
	{
		int line = 0;
		if (IsDisposed || item == null)
		{
			return;
		}
		try
		{
			await sem.WaitAsync();
			line = 1;
			_dic.TryAdd(key, item);
			if (_dic.Count() > _maxCollections)
			{
				line = 4;
				Task.Run(() => RaiseDataReadyAsync());
			}
			if (Count() > 0 && !TimeOutEnable)
			{
				line = 5;
				Task.Run(async delegate
				{
					await TimeOutTask();
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"add(IEnumerable<T> items) Line:{line}", "Add", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CollectionWatcher.cs", 111);
		}
		finally
		{
			sem.Release();
		}
	}

	private async Task RaiseDataReadyAsync()
	{
		await semReady.WaitAsync();
		try
		{
			if (!IsDisposed && this.DataReady != null && _dic != null && _dic.Count() > 0)
			{
				List<T> lst = _dic.Values?.ToList();
				_dic.Clear();
				Task.Run(() => this.DataReady(this, lst));
			}
		}
		catch (Exception ex)
		{
			semReady.Release();
			WebErrorLog.Instance.StartLog(ex, "", "RaiseDataReadyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CollectionWatcher.cs", 160);
		}
	}

	public int Count()
	{
		return _dic?.Count ?? 0;
	}

	public async Task FlushAsync()
	{
		await RaiseDataReadyAsync();
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_dic?.Clear();
				_dic = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CollectionWatcher.cs", 183);
		}
	}
}
