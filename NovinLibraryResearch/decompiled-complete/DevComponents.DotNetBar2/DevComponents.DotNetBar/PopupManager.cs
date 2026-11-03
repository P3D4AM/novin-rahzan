using System;
using System.Collections.Generic;
using System.Threading;

namespace DevComponents.DotNetBar;

public static class PopupManager
{
	private static List<WeakReference> _ᓓ;

	private static ReaderWriterLock ᓔ;

	static PopupManager()
	{
		_ᓓ = new List<WeakReference>();
		ᓔ = new ReaderWriterLock();
	}

	public static void RegisterPopup(IOwnerMenuSupport popupHost)
	{
		if (popupHost == null || ᓕ(popupHost) != null)
		{
			return;
		}
		WeakReference weakReference = new WeakReference(popupHost);
		weakReference.Target = popupHost;
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = ᓔ.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = ᓔ.UpgradeToWriterLock(-1);
		}
		else
		{
			ᓔ.AcquireWriterLock(-1);
		}
		try
		{
			_ᓓ.Add(weakReference);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				ᓔ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				ᓔ.ReleaseWriterLock();
			}
		}
		popupHost = null;
	}

	private static WeakReference ᓕ(IOwnerMenuSupport ᓖ)
	{
		List<WeakReference> ᓓ = _ᓓ;
		if (ᓓ == null)
		{
			return null;
		}
		ᓔ.AcquireReaderLock(-1);
		try
		{
			foreach (WeakReference item in ᓓ)
			{
				object target = item.Target;
				if (target != null && target.Equals(ᓖ))
				{
					return item;
				}
			}
		}
		finally
		{
			ᓔ.ReleaseReaderLock();
		}
		return null;
	}

	public static void UnregisterPopup(IOwnerMenuSupport popupHost)
	{
		if (popupHost == null)
		{
			return;
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = ᓔ.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = ᓔ.UpgradeToWriterLock(-1);
		}
		else
		{
			ᓔ.AcquireWriterLock(-1);
		}
		try
		{
			foreach (WeakReference item in _ᓓ)
			{
				object target = item.Target;
				if (target != null && target.Equals(popupHost))
				{
					_ᓓ.Remove(item);
					break;
				}
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				ᓔ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				ᓔ.ReleaseWriterLock();
			}
		}
	}

	public static void CloseAllPopups()
	{
		CloseAllPopups(null);
	}

	public static void CloseAllPopups(IOwnerMenuSupport excludeHost)
	{
		ᓔ.AcquireReaderLock(-1);
		WeakReference[] array;
		try
		{
			array = new WeakReference[_ᓓ.Count];
			_ᓓ.CopyTo(array);
		}
		finally
		{
			ᓔ.ReleaseReaderLock();
		}
		WeakReference[] array2 = array;
		foreach (WeakReference weakReference in array2)
		{
			object target = weakReference.Target;
			if (weakReference.IsAlive && target != null && target is IOwnerMenuSupport ownerMenuSupport && ownerMenuSupport != excludeHost)
			{
				ownerMenuSupport.ClosePopups();
			}
		}
	}
}
