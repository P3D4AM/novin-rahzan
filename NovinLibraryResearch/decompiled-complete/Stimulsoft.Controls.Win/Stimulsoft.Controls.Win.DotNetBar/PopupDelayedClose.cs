using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class PopupDelayedClose
{
	private Timer m_Timer;

	private ArrayList m_Popups = new ArrayList();

	private ReaderWriterLock m_PopupsLock = new ReaderWriterLock();

	public PopupDelayedClose()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		m_Timer = new Timer();
		m_Timer.Enabled = false;
		m_Timer.Interval = 800;
		m_Timer.Tick += TimerDelayedClose;
	}

	private void TimerDelayedClose(object sender, EventArgs e)
	{
		m_Timer.Stop();
		CloseAllPopups();
	}

	public void Dispose()
	{
		if (m_Timer != null)
		{
			m_Timer.Stop();
			CloseAllPopups();
			((Component)(object)m_Timer).Dispose();
			m_Timer = null;
		}
	}

	private void CloseAllPopups()
	{
		if (m_Popups.Count <= 0)
		{
			return;
		}
		bool isReaderLockHeld = m_PopupsLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = m_PopupsLock.UpgradeToWriterLock(-1);
		}
		else
		{
			m_PopupsLock.AcquireWriterLock(-1);
		}
		try
		{
			foreach (PopupItem popup in m_Popups)
			{
				popup.ClosePopup();
			}
			m_Popups.Clear();
		}
		finally
		{
			if (isReaderLockHeld)
			{
				m_PopupsLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				m_PopupsLock.ReleaseWriterLock();
			}
		}
	}

	public void DelayClose(PopupItem item)
	{
		if (m_Timer == null)
		{
			return;
		}
		bool isReaderLockHeld = m_PopupsLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = m_PopupsLock.UpgradeToWriterLock(-1);
		}
		else
		{
			m_PopupsLock.AcquireWriterLock(-1);
		}
		try
		{
			m_Popups.Add(item);
			m_Timer.Start();
		}
		finally
		{
			if (isReaderLockHeld)
			{
				m_PopupsLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				m_PopupsLock.ReleaseWriterLock();
			}
		}
	}

	public void EraseDelayClose()
	{
		bool isReaderLockHeld = m_PopupsLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = m_PopupsLock.UpgradeToWriterLock(-1);
		}
		else
		{
			m_PopupsLock.AcquireWriterLock(-1);
		}
		try
		{
			m_Popups.Clear();
		}
		finally
		{
			if (isReaderLockHeld)
			{
				m_PopupsLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				m_PopupsLock.ReleaseWriterLock();
			}
		}
	}
}
