using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal class ThrottlingCache
{
	internal const int DefaultCleanupIntervalMs = 300000;

	private volatile bool _cleanupInProgress;

	private static readonly object _padlock = new object();

	private readonly TimeSpan s_cleanupCacheInterval;

	private DateTimeOffset _lastCleanupTime = DateTimeOffset.UtcNow;

	private readonly ConcurrentDictionary<string, ThrottlingCacheEntry> _cache = new ConcurrentDictionary<string, ThrottlingCacheEntry>();

	internal ConcurrentDictionary<string, ThrottlingCacheEntry> CacheForTest => _cache;

	public ThrottlingCache(int? customCleanupIntervalMs = null)
	{
		s_cleanupCacheInterval = (customCleanupIntervalMs.HasValue ? TimeSpan.FromMilliseconds(customCleanupIntervalMs.Value) : TimeSpan.FromMilliseconds(300000.0));
	}

	public void AddAndCleanup(string key, ThrottlingCacheEntry entry, ICoreLogger logger)
	{
		_cache.AddOrUpdate(key, entry, (string _, ThrottlingCacheEntry oldEntry) => (!(entry.CreationTime > oldEntry.CreationTime)) ? oldEntry : entry);
		CleanCache(logger);
	}

	public bool TryGetOrRemoveExpired(string key, ICoreLogger logger, out MsalServiceException ex)
	{
		ex = null;
		if (_cache.TryGetValue(key, out var value))
		{
			logger.Info($"[Throttling] Entry found. Creation: {value.CreationTime} Expiration: {value.ExpirationTime} ");
			if (value.IsExpired)
			{
				logger.Info("[Throttling] Removing entry because it is expired");
				_cache.TryRemove(key, out var _);
				return false;
			}
			logger.InfoPii("[Throttling] Returning valid entry for key " + key, "[Throttling] Returning valid entry.");
			ex = value.Exception;
			return true;
		}
		return false;
	}

	public void Clear()
	{
		_cache.Clear();
	}

	public bool IsEmpty()
	{
		return !_cache.Any();
	}

	private void CleanCache(ICoreLogger logger)
	{
		if (!(_lastCleanupTime + s_cleanupCacheInterval < DateTimeOffset.UtcNow) || _cleanupInProgress)
		{
			return;
		}
		lock (_padlock)
		{
			if (!_cleanupInProgress)
			{
				logger.Verbose($"[Throttling] Cache size before cleaning up {_cache.Count}");
				_cleanupInProgress = true;
				CleanupCacheNoLocks();
				_lastCleanupTime = DateTimeOffset.UtcNow;
				_cleanupInProgress = false;
				logger.Verbose($"[Throttling] Cache size after cleaning up {_cache.Count}");
			}
		}
	}

	private void CleanupCacheNoLocks()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, ThrottlingCacheEntry> item in _cache)
		{
			if (item.Value.IsExpired)
			{
				list.Add(item.Key);
			}
		}
		foreach (string item2 in list)
		{
			_cache.TryRemove(item2, out var _);
		}
	}
}
