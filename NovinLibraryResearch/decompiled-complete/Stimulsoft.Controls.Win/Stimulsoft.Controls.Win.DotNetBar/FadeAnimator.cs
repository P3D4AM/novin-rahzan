using System;
using System.Collections;
using System.Threading;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class FadeAnimator
{
	private class FadeInfo
	{
		private object m_Caller;

		private EventHandler m_OnFadeChangedHandler;

		public EventHandler FadeChangedHandler
		{
			get
			{
				return m_OnFadeChangedHandler;
			}
			set
			{
				m_OnFadeChangedHandler = value;
			}
		}

		internal object Caller => m_Caller;

		public FadeInfo(object caller)
		{
			m_Caller = caller;
		}

		protected void OnFadeChanged(EventArgs e)
		{
			if (m_OnFadeChangedHandler != null)
			{
				m_OnFadeChangedHandler(m_Caller, e);
			}
		}

		public void Fade()
		{
			OnFadeChanged(EventArgs.Empty);
		}
	}

	private static Thread fadeThread;

	private static ArrayList fadeInfoList;

	private static ReaderWriterLock rwImgListLock;

	[ThreadStatic]
	private static int threadWriterLockWaitCount;

	static FadeAnimator()
	{
		rwImgListLock = new ReaderWriterLock();
	}

	private FadeAnimator()
	{
	}

	public static void Fade(object caller, EventHandler onFadeChangedHandler)
	{
		if (caller == null)
		{
			return;
		}
		FadeInfo fadeInfo = null;
		lock (caller)
		{
			fadeInfo = new FadeInfo(caller);
		}
		StopFade(caller, onFadeChangedHandler);
		bool isReaderLockHeld = rwImgListLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		threadWriterLockWaitCount++;
		try
		{
			if (isReaderLockHeld)
			{
				lockCookie = rwImgListLock.UpgradeToWriterLock(-1);
			}
			else
			{
				rwImgListLock.AcquireWriterLock(-1);
			}
		}
		finally
		{
			threadWriterLockWaitCount--;
		}
		try
		{
			if (fadeInfoList == null)
			{
				fadeInfoList = new ArrayList();
			}
			fadeInfo.FadeChangedHandler = onFadeChangedHandler;
			fadeInfoList.Add(fadeInfo);
			if (fadeThread == null)
			{
				fadeThread = new Thread(AnimateFade50ms);
				fadeThread.Name = typeof(FadeAnimator).Name;
				fadeThread.IsBackground = true;
				fadeThread.Start();
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				rwImgListLock.ReleaseWriterLock();
			}
		}
	}

	private static void AnimateFade50ms()
	{
		while (true)
		{
			rwImgListLock.AcquireReaderLock(-1);
			try
			{
				for (int i = 0; i < fadeInfoList.Count; i++)
				{
					FadeInfo fadeInfo = fadeInfoList[i] as FadeInfo;
					fadeInfo.Fade();
				}
			}
			finally
			{
				rwImgListLock.ReleaseReaderLock();
			}
			Thread.Sleep(40);
		}
	}

	public static void StopFade(object caller, EventHandler onFrameChangedHandler)
	{
		if (caller == null || fadeInfoList == null)
		{
			return;
		}
		bool isReaderLockHeld = rwImgListLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		threadWriterLockWaitCount++;
		try
		{
			if (isReaderLockHeld)
			{
				lockCookie = rwImgListLock.UpgradeToWriterLock(-1);
			}
			else
			{
				rwImgListLock.AcquireWriterLock(-1);
			}
		}
		finally
		{
			threadWriterLockWaitCount--;
		}
		try
		{
			for (int i = 0; i < fadeInfoList.Count; i++)
			{
				FadeInfo fadeInfo = fadeInfoList[i] as FadeInfo;
				if (caller == fadeInfo.Caller)
				{
					if (onFrameChangedHandler == fadeInfo.FadeChangedHandler || (onFrameChangedHandler != null && onFrameChangedHandler.Equals(fadeInfo.FadeChangedHandler)))
					{
						fadeInfoList.Remove(fadeInfo);
					}
					break;
				}
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				rwImgListLock.ReleaseWriterLock();
			}
		}
	}
}
