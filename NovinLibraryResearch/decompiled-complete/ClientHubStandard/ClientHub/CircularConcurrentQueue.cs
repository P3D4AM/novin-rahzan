using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PacketParser.Entities;

namespace ClientHub;

internal class CircularConcurrentQueue<T> : IEnumerable<T>, IEnumerable
{
	private ConcurrentQueue<T> items;

	public int Capacity { get; set; }

	public int Count => items.Count;

	public CircularConcurrentQueue(int capacity)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs"))
		{
			try
			{
				items = new ConcurrentQueue<T>();
				Capacity = capacity;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs", 26);
			}
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		using (new JaegerMonitor("GetEnumerator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs"))
		{
			try
			{
				return items.GetEnumerator();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetEnumerator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs", 40);
				return null;
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		using (new JaegerMonitor("GetEnumerator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs"))
		{
			try
			{
				return GetEnumerator();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetEnumerator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs", 55);
				return null;
			}
		}
	}

	public bool TryEnqueue(T item)
	{
		try
		{
			if (items.Count >= Capacity)
			{
				if (!items.TryDequeue(out var _))
				{
					return false;
				}
				items.Enqueue(item);
			}
			else
			{
				items.Enqueue(item);
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TryEnqueue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs", 79);
			return false;
		}
	}

	public bool TryDequeue(out T result)
	{
		using (new JaegerMonitor("TryDequeue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs"))
		{
			try
			{
				return items.TryDequeue(out result);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TryDequeue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\CircularConcurrentQueue.cs", 93);
				result = default(T);
				return false;
			}
		}
	}
}
