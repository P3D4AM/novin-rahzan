using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory;

internal class CacheEntry : ICacheEntry, IDisposable
{
	private bool _disposed;

	private static readonly Action<object> ExpirationCallback = ExpirationTokensExpired;

	private readonly Action<CacheEntry> _notifyCacheOfExpiration;

	private readonly Action<CacheEntry> _notifyCacheEntryCommit;

	private IList<IDisposable> _expirationTokenRegistrations;

	private IList<PostEvictionCallbackRegistration> _postEvictionCallbacks;

	private bool _isExpired;

	internal IList<IChangeToken> _expirationTokens;

	internal DateTimeOffset? _absoluteExpiration;

	internal TimeSpan? _absoluteExpirationRelativeToNow;

	private TimeSpan? _slidingExpiration;

	private long? _size;

	private IDisposable _scope;

	private object _value;

	private bool _valueHasBeenSet;

	internal readonly object _lock = new object();

	public DateTimeOffset? AbsoluteExpiration
	{
		get
		{
			return _absoluteExpiration;
		}
		set
		{
			_absoluteExpiration = value;
		}
	}

	public TimeSpan? AbsoluteExpirationRelativeToNow
	{
		get
		{
			return _absoluteExpirationRelativeToNow;
		}
		set
		{
			if (value <= TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("AbsoluteExpirationRelativeToNow", value, "The relative expiration value must be positive.");
			}
			_absoluteExpirationRelativeToNow = value;
		}
	}

	public TimeSpan? SlidingExpiration
	{
		get
		{
			return _slidingExpiration;
		}
		set
		{
			if (value <= TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("SlidingExpiration", value, "The sliding expiration value must be positive.");
			}
			_slidingExpiration = value;
		}
	}

	public IList<IChangeToken> ExpirationTokens
	{
		get
		{
			if (_expirationTokens == null)
			{
				_expirationTokens = new List<IChangeToken>();
			}
			return _expirationTokens;
		}
	}

	public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks
	{
		get
		{
			if (_postEvictionCallbacks == null)
			{
				_postEvictionCallbacks = new List<PostEvictionCallbackRegistration>();
			}
			return _postEvictionCallbacks;
		}
	}

	public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;


	public long? Size
	{
		get
		{
			return _size;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", value, "value must be non-negative.");
			}
			_size = value;
		}
	}

	public object Key { get; private set; }

	public object Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			_valueHasBeenSet = true;
		}
	}

	internal DateTimeOffset LastAccessed { get; set; }

	internal EvictionReason EvictionReason { get; private set; }

	internal CacheEntry(object key, Action<CacheEntry> notifyCacheEntryCommit, Action<CacheEntry> notifyCacheOfExpiration)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (notifyCacheEntryCommit == null)
		{
			throw new ArgumentNullException("notifyCacheEntryCommit");
		}
		if (notifyCacheOfExpiration == null)
		{
			throw new ArgumentNullException("notifyCacheOfExpiration");
		}
		Key = key;
		_notifyCacheEntryCommit = notifyCacheEntryCommit;
		_notifyCacheOfExpiration = notifyCacheOfExpiration;
		_scope = CacheEntryHelper.EnterScope(this);
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			_scope.Dispose();
			_scope = null;
			if (_valueHasBeenSet)
			{
				_notifyCacheEntryCommit(this);
				PropagateOptions(CacheEntryHelper.Current);
			}
		}
	}

	internal bool CheckExpired(DateTimeOffset now)
	{
		if (!_isExpired && !CheckForExpiredTime(now))
		{
			return CheckForExpiredTokens();
		}
		return true;
	}

	internal void SetExpired(EvictionReason reason)
	{
		if (EvictionReason == EvictionReason.None)
		{
			EvictionReason = reason;
		}
		_isExpired = true;
		DetachTokens();
	}

	private bool CheckForExpiredTime(DateTimeOffset now)
	{
		if (_absoluteExpiration.HasValue && _absoluteExpiration.Value <= now)
		{
			SetExpired(EvictionReason.Expired);
			return true;
		}
		if (_slidingExpiration.HasValue)
		{
			TimeSpan value = now - LastAccessed;
			TimeSpan? slidingExpiration = _slidingExpiration;
			if (value >= slidingExpiration)
			{
				SetExpired(EvictionReason.Expired);
				return true;
			}
		}
		return false;
	}

	internal bool CheckForExpiredTokens()
	{
		if (_expirationTokens != null)
		{
			for (int i = 0; i < _expirationTokens.Count; i++)
			{
				if (_expirationTokens[i].HasChanged)
				{
					SetExpired(EvictionReason.TokenExpired);
					return true;
				}
			}
		}
		return false;
	}

	internal void AttachTokens()
	{
		if (_expirationTokens == null)
		{
			return;
		}
		lock (_lock)
		{
			for (int i = 0; i < _expirationTokens.Count; i++)
			{
				IChangeToken changeToken = _expirationTokens[i];
				if (changeToken.ActiveChangeCallbacks)
				{
					if (_expirationTokenRegistrations == null)
					{
						_expirationTokenRegistrations = new List<IDisposable>(1);
					}
					IDisposable item = changeToken.RegisterChangeCallback(ExpirationCallback, this);
					_expirationTokenRegistrations.Add(item);
				}
			}
		}
	}

	private static void ExpirationTokensExpired(object obj)
	{
		Task.Factory.StartNew(delegate(object state)
		{
			CacheEntry cacheEntry = (CacheEntry)state;
			cacheEntry.SetExpired(EvictionReason.TokenExpired);
			cacheEntry._notifyCacheOfExpiration(cacheEntry);
		}, obj, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
	}

	private void DetachTokens()
	{
		lock (_lock)
		{
			IList<IDisposable> expirationTokenRegistrations = _expirationTokenRegistrations;
			if (expirationTokenRegistrations != null)
			{
				_expirationTokenRegistrations = null;
				for (int i = 0; i < expirationTokenRegistrations.Count; i++)
				{
					expirationTokenRegistrations[i].Dispose();
				}
			}
		}
	}

	internal void InvokeEvictionCallbacks()
	{
		if (_postEvictionCallbacks != null)
		{
			Task.Factory.StartNew(delegate(object state)
			{
				InvokeCallbacks((CacheEntry)state);
			}, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}
	}

	private static void InvokeCallbacks(CacheEntry entry)
	{
		IList<PostEvictionCallbackRegistration> list = Interlocked.Exchange(ref entry._postEvictionCallbacks, null);
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			PostEvictionCallbackRegistration postEvictionCallbackRegistration = list[i];
			try
			{
				postEvictionCallbackRegistration.EvictionCallback?.Invoke(entry.Key, entry.Value, entry.EvictionReason, postEvictionCallbackRegistration.State);
			}
			catch (Exception)
			{
			}
		}
	}

	internal void PropagateOptions(CacheEntry parent)
	{
		if (parent == null)
		{
			return;
		}
		if (_expirationTokens != null)
		{
			lock (_lock)
			{
				lock (parent._lock)
				{
					foreach (IChangeToken expirationToken in _expirationTokens)
					{
						parent.AddExpirationToken(expirationToken);
					}
				}
			}
		}
		if (_absoluteExpiration.HasValue && (!parent._absoluteExpiration.HasValue || _absoluteExpiration < parent._absoluteExpiration))
		{
			parent._absoluteExpiration = _absoluteExpiration;
		}
	}
}
