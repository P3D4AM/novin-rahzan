using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Interfaces;
using PacketParser.Entities;

namespace EntityCache;

public class Obsevable : IObsevable
{
	private HashSet<IObserver> observers = new HashSet<IObserver>();

	private bool isDisposed;

	public Task DisposeObservers()
	{
		try
		{
			if (!isDisposed && observers != null)
			{
				isDisposed = true;
				foreach (IObserver observer in observers)
				{
					observer?.Dispose();
				}
				observers = new HashSet<IObserver>();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeObservers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Obsevable.cs", 28);
		}
		return Task.CompletedTask;
	}

	public void Subscribe(IObserver observer)
	{
		try
		{
			observers.Add(observer);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Subscribe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Obsevable.cs", 38);
		}
	}

	public void UnSubscribe(IObserver observer)
	{
		try
		{
			observers.Remove(observer);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UnSubscribe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Obsevable.cs", 47);
		}
	}
}
