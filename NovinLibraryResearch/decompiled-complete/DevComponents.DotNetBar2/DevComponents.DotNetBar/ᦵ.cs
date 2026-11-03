using System;
using System.Collections;
using System.Threading;

namespace DevComponents.DotNetBar;

internal class ᦵ
{
	private class ᦿ
	{
		private object ᧂ;

		private EventHandler ᧃ;

		public EventHandler ᧀ
		{
			get
			{
				return ᧃ;
			}
			set
			{
				ᧃ = value;
			}
		}

		internal object ᧁ
		{
			get
			{
				return ᧂ;
			}
			set
			{
				ᧂ = value;
			}
		}

		public ᦿ(object caller)
		{
			ᧂ = caller;
		}

		protected void _໑(EventArgs ؿ)
		{
			if (ᧃ != null)
			{
				ᧃ(ᧂ, ؿ);
			}
		}

		public void ᦺ()
		{
			_໑(EventArgs.Empty);
		}
	}

	private static Thread ᦶ;

	private static ArrayList ᦷ;

	private static ReaderWriterLock ᦸ;

	[ThreadStatic]
	private static int ᦹ;

	static ᦵ()
	{
		ᦸ = new ReaderWriterLock();
	}

	private ᦵ()
	{
	}

	public static void ᦺ(object ᦻ, EventHandler ᦼ)
	{
		if (!BarUtilities.FadeAnimatorEnabled || ᦻ == null)
		{
			return;
		}
		ᦿ ᦿ = null;
		lock (ᦻ)
		{
			ᦿ = new ᦿ(ᦻ);
		}
		_0ECF(ᦻ, ᦼ);
		bool isReaderLockHeld = ᦸ.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		ᦹ++;
		try
		{
			if (isReaderLockHeld)
			{
				lockCookie = ᦸ.UpgradeToWriterLock(-1);
			}
			else
			{
				ᦸ.AcquireWriterLock(-1);
			}
		}
		finally
		{
			ᦹ--;
		}
		try
		{
			if (ᦷ == null)
			{
				ᦷ = new ArrayList();
			}
			ᦿ.ᧀ = ᦼ;
			ᦷ.Add(ᦿ);
			if (ᦶ == null)
			{
				ᦶ = new Thread(ᦽ);
				ᦶ.Name = typeof(ᦵ).Name;
				ᦶ.IsBackground = true;
				ᦶ.Start();
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				ᦸ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				ᦸ.ReleaseWriterLock();
			}
		}
	}

	private static void ᦽ()
	{
		while (true)
		{
			ᦸ.AcquireReaderLock(-1);
			try
			{
				for (int i = 0; i < ᦷ.Count; i++)
				{
					ᦿ ᦿ = ᦷ[i] as ᦿ;
					ᦿ.ᦺ();
				}
			}
			finally
			{
				ᦸ.ReleaseReaderLock();
			}
			try
			{
				using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
				manualResetEvent.WaitOne(40);
			}
			catch
			{
			}
		}
	}

	public static void _0ECF(object ᦻ, EventHandler ᦾ)
	{
		if (!BarUtilities.FadeAnimatorEnabled || ᦻ == null || ᦷ == null)
		{
			return;
		}
		bool isReaderLockHeld = ᦸ.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		ᦹ++;
		try
		{
			if (isReaderLockHeld)
			{
				lockCookie = ᦸ.UpgradeToWriterLock(-1);
			}
			else
			{
				ᦸ.AcquireWriterLock(-1);
			}
		}
		finally
		{
			ᦹ--;
		}
		try
		{
			for (int i = 0; i < ᦷ.Count; i++)
			{
				ᦿ ᦿ = ᦷ[i] as ᦿ;
				if (ᦻ == ᦿ.ᧁ)
				{
					if (ᦾ == ᦿ.ᧀ || (ᦾ != null && ᦾ.Equals(ᦿ.ᧀ)))
					{
						ᦷ.Remove(ᦿ);
						ᦿ.ᧀ = null;
						ᦿ.ᧁ = null;
					}
					break;
				}
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				ᦸ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				ᦸ.ReleaseWriterLock();
			}
		}
	}
}
