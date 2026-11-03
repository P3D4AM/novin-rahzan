using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser;

public class BufferConcurrentDictonary<T, U> : IDisposable
{
	private ConcurrentDictionary<T, U> _dic = new ConcurrentDictionary<T, U>();

	private ConcurrentDictionary<T, DateTime> _dic_Modified = new ConcurrentDictionary<T, DateTime>();

	private CancellationTokenSource _token = new CancellationTokenSource();

	private short _BufferringMinutes = 5;

	private bool _DisposeIsRunning;

	public BufferConcurrentDictonary(short BufferringMinutes = 5)
	{
		_BufferringMinutes = BufferringMinutes;
	}

	public bool GetValue(T Key, out U value)
	{
		return _dic.TryGetValue(Key, out value);
	}

	public bool SetValue(T Key, U Value)
	{
		_dic_Modified.TryRemove(Key, out var _);
		_dic_Modified.TryAdd(Key, DateTime.Now);
		_dic.TryRemove(Key, out var _);
		StartDisposeTimer();
		return _dic.TryAdd(Key, Value);
	}

	private async Task StartDisposeTimer()
	{
		if (_DisposeIsRunning)
		{
			return;
		}
		_DisposeIsRunning = true;
		try
		{
			await Task.Delay(checked(_BufferringMinutes * 60 * 1000), _token.Token);
			foreach (T key in _dic.Keys)
			{
				if (!_dic_Modified.TryGetValue(key, out var value))
				{
					_dic.TryRemove(key, out var _);
				}
				else if (value.AddMinutes(_BufferringMinutes) > DateTime.Now)
				{
					_dic.TryRemove(key, out var _);
					_dic_Modified.TryRemove(key, out var _);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartDisposeTimer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BufferConcurrentDictonary.cs", 58);
		}
		finally
		{
			_DisposeIsRunning = false;
		}
		ConcurrentDictionary<T, U> dic = _dic;
		if (dic != null && dic.Count > 0)
		{
			StartDisposeTimer();
		}
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}
}
