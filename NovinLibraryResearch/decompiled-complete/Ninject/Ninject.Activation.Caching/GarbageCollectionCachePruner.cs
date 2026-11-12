using System;
using System.Collections.Generic;
using System.Threading;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;

namespace Ninject.Activation.Caching;

public class GarbageCollectionCachePruner : NinjectComponent, ICachePruner, INinjectComponent, IDisposable
{
	private readonly WeakReference indicator = new WeakReference(new object());

	private readonly List<IPruneable> caches = new List<IPruneable>();

	private Timer timer;

	private bool stop;

	public override void Dispose(bool disposing)
	{
		if (disposing && !base.IsDisposed && timer != null)
		{
			Stop();
		}
		base.Dispose(disposing);
	}

	public void Start(IPruneable pruneable)
	{
		Ensure.ArgumentNotNull(pruneable, "pruneable");
		caches.Add(pruneable);
		if (timer == null)
		{
			timer = new Timer(PruneCacheIfGarbageCollectorHasRun, null, GetTimeoutInMilliseconds(), -1);
		}
	}

	public void Stop()
	{
		lock (this)
		{
			stop = true;
		}
		using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		timer.Dispose(manualResetEvent);
		manualResetEvent.WaitOne();
		timer = null;
		caches.Clear();
	}

	private void PruneCacheIfGarbageCollectorHasRun(object state)
	{
		lock (this)
		{
			if (stop)
			{
				return;
			}
			try
			{
				if (!indicator.IsAlive)
				{
					caches.Map(delegate(IPruneable cache)
					{
						cache.Prune();
					});
					indicator.Target = new object();
				}
			}
			finally
			{
				timer.Change(GetTimeoutInMilliseconds(), -1);
			}
		}
	}

	private int GetTimeoutInMilliseconds()
	{
		TimeSpan cachePruningInterval = base.Settings.CachePruningInterval;
		if (!(cachePruningInterval == TimeSpan.MaxValue))
		{
			return (int)cachePruningInterval.TotalMilliseconds;
		}
		return -1;
	}
}
