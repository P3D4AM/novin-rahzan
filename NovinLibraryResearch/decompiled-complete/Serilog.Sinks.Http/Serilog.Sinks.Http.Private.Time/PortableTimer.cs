using System;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Sinks.Http.Private.Time;

public class PortableTimer : IDisposable
{
	private readonly object syncRoot = new object();

	private readonly Func<Task> onTick;

	private readonly Timer timer;

	private bool running;

	private bool disposed;

	public PortableTimer(Func<Task> onTick)
	{
		this.onTick = onTick ?? throw new ArgumentNullException("onTick");
		timer = new Timer(delegate
		{
			OnTick();
		}, null, -1, -1);
	}

	public void Start(TimeSpan interval)
	{
		if (interval < TimeSpan.Zero)
		{
			throw new ArgumentOutOfRangeException("interval");
		}
		lock (syncRoot)
		{
			if (disposed)
			{
				throw new ObjectDisposedException("PortableTimer");
			}
			timer.Change(interval, Timeout.InfiniteTimeSpan);
		}
	}

	public void Dispose()
	{
		lock (syncRoot)
		{
			if (!disposed)
			{
				while (running)
				{
					Monitor.Wait(syncRoot);
				}
				timer.Dispose();
				disposed = true;
			}
		}
	}

	private async void OnTick()
	{
		try
		{
			lock (syncRoot)
			{
				if (disposed)
				{
					return;
				}
				if (running)
				{
					Monitor.Wait(syncRoot);
					if (disposed)
					{
						return;
					}
				}
				running = true;
			}
			await onTick();
		}
		finally
		{
			lock (syncRoot)
			{
				running = false;
				Monitor.PulseAll(syncRoot);
			}
		}
	}
}
