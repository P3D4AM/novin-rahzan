using System;
using System.Threading;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThreadUIOperation
{
	private bool m_IsActive;

	private ReaderWriterLock m_FadeImageLock = new ReaderWriterLock();

	private EventHandler m_UpdateUIHandler;

	private EventHandler m_RecordStartStateHandler;

	private EventHandler m_CleanupHandler;

	public bool IsActive => m_IsActive;

	public ThreadUIOperation(EventHandler recordStartState, EventHandler updateUIHandler, EventHandler cleanup)
	{
		m_RecordStartStateHandler = recordStartState;
		m_UpdateUIHandler = updateUIHandler;
		m_CleanupHandler = cleanup;
	}

	public void Start()
	{
		m_FadeImageLock.AcquireReaderLock(-1);
		try
		{
			if (m_IsActive)
			{
				return;
			}
		}
		finally
		{
			m_FadeImageLock.ReleaseReaderLock();
		}
		bool isReaderLockHeld = m_FadeImageLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = m_FadeImageLock.UpgradeToWriterLock(-1);
		}
		else
		{
			m_FadeImageLock.AcquireWriterLock(-1);
		}
		try
		{
			m_IsActive = true;
			m_RecordStartStateHandler(this, new EventArgs());
		}
		finally
		{
			if (isReaderLockHeld)
			{
				m_FadeImageLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				m_FadeImageLock.ReleaseWriterLock();
			}
		}
		FadeAnimator.Fade(this, OnFadeChanged);
		m_IsActive = true;
	}

	public void Stop()
	{
		m_FadeImageLock.AcquireReaderLock(-1);
		try
		{
			if (!m_IsActive)
			{
				return;
			}
		}
		finally
		{
			m_FadeImageLock.ReleaseReaderLock();
		}
		FadeAnimator.StopFade(this, OnFadeChanged);
		bool isReaderLockHeld = m_FadeImageLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = m_FadeImageLock.UpgradeToWriterLock(-1);
		}
		else
		{
			m_FadeImageLock.AcquireWriterLock(-1);
		}
		try
		{
			m_IsActive = false;
			m_CleanupHandler(this, new EventArgs());
		}
		finally
		{
			if (isReaderLockHeld)
			{
				m_FadeImageLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				m_FadeImageLock.ReleaseWriterLock();
			}
		}
	}

	private void OnFadeChanged(object sender, EventArgs e)
	{
		m_UpdateUIHandler(this, new EventArgs());
	}

	public void StartReadOperation()
	{
		m_FadeImageLock.AcquireReaderLock(-1);
	}

	public void EndReadOperation()
	{
		m_FadeImageLock.ReleaseReaderLock();
	}
}
