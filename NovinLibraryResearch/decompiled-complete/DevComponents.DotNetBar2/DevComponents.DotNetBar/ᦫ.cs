using System;
using System.Threading;

namespace DevComponents.DotNetBar;

internal class ᦫ
{
	private bool _19AD;

	private ReaderWriterLock າ = new ReaderWriterLock();

	private EventHandler _19AE;

	private EventHandler _19AF;

	private EventHandler ᦰ;

	public bool _19AC => _19AD;

	public ᦫ(EventHandler recordStartState, EventHandler updateUIHandler, EventHandler cleanup)
	{
		_19AF = recordStartState;
		_19AE = updateUIHandler;
		ᦰ = cleanup;
	}

	public void ᦱ()
	{
		າ.AcquireReaderLock(-1);
		try
		{
			if (_19AD)
			{
				return;
			}
		}
		finally
		{
			າ.ReleaseReaderLock();
		}
		bool isReaderLockHeld = າ.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = າ.UpgradeToWriterLock(-1);
		}
		else
		{
			າ.AcquireWriterLock(-1);
		}
		try
		{
			_19AD = true;
			_19AF(this, new EventArgs());
		}
		finally
		{
			if (isReaderLockHeld)
			{
				າ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				າ.ReleaseWriterLock();
			}
		}
		ᦵ.ᦺ(this, _໑);
		_19AD = true;
	}

	public void ᦲ()
	{
		າ.AcquireReaderLock(-1);
		try
		{
			if (!_19AD)
			{
				return;
			}
		}
		finally
		{
			າ.ReleaseReaderLock();
		}
		ᦵ._0ECF(this, _໑);
		bool isReaderLockHeld = າ.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = າ.UpgradeToWriterLock(-1);
		}
		else
		{
			າ.AcquireWriterLock(-1);
		}
		try
		{
			_19AD = false;
			ᦰ(this, new EventArgs());
		}
		finally
		{
			if (isReaderLockHeld)
			{
				າ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				າ.ReleaseWriterLock();
			}
		}
	}

	private void _໑(object _٠, EventArgs ؿ)
	{
		_19AE(this, new EventArgs());
	}

	public void ᦳ()
	{
		າ.AcquireReaderLock(-1);
	}

	public void ᦴ()
	{
		າ.ReleaseReaderLock();
	}
}
