using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Caching.Memory;

public class MemoryCache : IMemoryCache, IDisposable
{
	private readonly ConcurrentDictionary<object, CacheEntry> _entries;

	private long _cacheSize;

	private bool _disposed;

	private ILogger _logger;

	private readonly Action<CacheEntry> _setEntry;

	private readonly Action<CacheEntry> _entryExpirationNotification;

	private readonly MemoryCacheOptions _options;

	private DateTimeOffset _lastExpirationScan;

	public int Count => _entries.Count;

	internal long Size => Interlocked.Read(ref _cacheSize);

	private ICollection<KeyValuePair<object, CacheEntry>> EntriesCollection => _entries;

	public MemoryCache(IOptions<MemoryCacheOptions> optionsAccessor)
		: this(optionsAccessor, NullLoggerFactory.Instance)
	{
	}

	public MemoryCache(IOptions<MemoryCacheOptions> optionsAccessor, ILoggerFactory loggerFactory)
	{
		if (optionsAccessor == null)
		{
			throw new ArgumentNullException("optionsAccessor");
		}
		if (loggerFactory == null)
		{
			throw new ArgumentNullException("loggerFactory");
		}
		_options = optionsAccessor.Value;
		_logger = loggerFactory.CreateLogger<MemoryCache>();
		_entries = new ConcurrentDictionary<object, CacheEntry>();
		_setEntry = SetEntry;
		_entryExpirationNotification = EntryExpired;
		if (_options.Clock == null)
		{
			_options.Clock = new SystemClock();
		}
		_lastExpirationScan = _options.Clock.UtcNow;
	}

	~MemoryCache()
	{
		Dispose(disposing: false);
	}

	public ICacheEntry CreateEntry(object key)
	{
		CheckDisposed();
		ValidateCacheKey(key);
		return new CacheEntry(key, _setEntry, _entryExpirationNotification);
	}

	private void SetEntry(CacheEntry entry)
	{
		if (_disposed)
		{
			return;
		}
		if (_options.SizeLimit.HasValue && !entry.Size.HasValue)
		{
			throw new InvalidOperationException("Cache entry must specify a value for Size when SizeLimit is set.");
		}
		DateTimeOffset utcNow = _options.Clock.UtcNow;
		DateTimeOffset? absoluteExpiration = null;
		if (entry._absoluteExpirationRelativeToNow.HasValue)
		{
			DateTimeOffset value = utcNow;
			TimeSpan? absoluteExpirationRelativeToNow = entry._absoluteExpirationRelativeToNow;
			absoluteExpiration = value + absoluteExpirationRelativeToNow;
		}
		else if (entry._absoluteExpiration.HasValue)
		{
			absoluteExpiration = entry._absoluteExpiration;
		}
		if (absoluteExpiration.HasValue && (!entry._absoluteExpiration.HasValue || absoluteExpiration.Value < entry._absoluteExpiration.Value))
		{
			entry._absoluteExpiration = absoluteExpiration;
		}
		entry.LastAccessed = utcNow;
		if (_entries.TryGetValue(entry.Key, out var value2))
		{
			value2.SetExpired(EvictionReason.Replaced);
		}
		bool flag = UpdateCacheSizeExceedsCapacity(entry);
		if (!entry.CheckExpired(utcNow) && !flag)
		{
			bool flag2 = false;
			if (value2 == null)
			{
				flag2 = _entries.TryAdd(entry.Key, entry);
			}
			else
			{
				flag2 = _entries.TryUpdate(entry.Key, entry, value2);
				if (flag2)
				{
					if (_options.SizeLimit.HasValue)
					{
						Interlocked.Add(ref _cacheSize, -value2.Size.Value);
					}
				}
				else
				{
					flag2 = _entries.TryAdd(entry.Key, entry);
				}
			}
			if (flag2)
			{
				entry.AttachTokens();
			}
			else
			{
				if (_options.SizeLimit.HasValue)
				{
					Interlocked.Add(ref _cacheSize, -entry.Size.Value);
				}
				entry.SetExpired(EvictionReason.Replaced);
				entry.InvokeEvictionCallbacks();
			}
			value2?.InvokeEvictionCallbacks();
		}
		else
		{
			if (flag)
			{
				entry.SetExpired(EvictionReason.Capacity);
				TriggerOvercapacityCompaction();
			}
			entry.InvokeEvictionCallbacks();
			if (value2 != null)
			{
				RemoveEntry(value2);
			}
		}
		StartScanForExpiredItems();
	}

	public bool TryGetValue(object key, out object result)
	{
		ValidateCacheKey(key);
		CheckDisposed();
		result = null;
		DateTimeOffset utcNow = _options.Clock.UtcNow;
		bool result2 = false;
		if (_entries.TryGetValue(key, out var value))
		{
			if (value.CheckExpired(utcNow) && value.EvictionReason != EvictionReason.Replaced)
			{
				RemoveEntry(value);
			}
			else
			{
				result2 = true;
				value.LastAccessed = utcNow;
				result = value.Value;
				value.PropagateOptions(CacheEntryHelper.Current);
			}
		}
		StartScanForExpiredItems();
		return result2;
	}

	public void Remove(object key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		CheckDisposed();
		if (_entries.TryRemove(key, out var value))
		{
			if (_options.SizeLimit.HasValue)
			{
				Interlocked.Add(ref _cacheSize, -value.Size.Value);
			}
			value.SetExpired(EvictionReason.Removed);
			value.InvokeEvictionCallbacks();
		}
		StartScanForExpiredItems();
	}

	private void RemoveEntry(CacheEntry entry)
	{
		if (EntriesCollection.Remove(new KeyValuePair<object, CacheEntry>(entry.Key, entry)))
		{
			if (_options.SizeLimit.HasValue)
			{
				Interlocked.Add(ref _cacheSize, -entry.Size.Value);
			}
			entry.InvokeEvictionCallbacks();
		}
	}

	private void EntryExpired(CacheEntry entry)
	{
		RemoveEntry(entry);
		StartScanForExpiredItems();
	}

	private void StartScanForExpiredItems()
	{
		DateTimeOffset utcNow = _options.Clock.UtcNow;
		if (_options.ExpirationScanFrequency < utcNow - _lastExpirationScan)
		{
			_lastExpirationScan = utcNow;
			Task.Factory.StartNew(delegate(object state)
			{
				ScanForExpiredItems((MemoryCache)state);
			}, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}
	}

	private static void ScanForExpiredItems(MemoryCache cache)
	{
		DateTimeOffset utcNow = cache._options.Clock.UtcNow;
		foreach (CacheEntry value in cache._entries.Values)
		{
			if (value.CheckExpired(utcNow))
			{
				cache.RemoveEntry(value);
			}
		}
	}

	private bool UpdateCacheSizeExceedsCapacity(CacheEntry entry)
	{
		if (!_options.SizeLimit.HasValue)
		{
			return false;
		}
		long num = 0L;
		for (int i = 0; i < 100; i++)
		{
			long num2 = Interlocked.Read(ref _cacheSize);
			num = num2 + entry.Size.Value;
			if (num < 0 || num > _options.SizeLimit)
			{
				return true;
			}
			if (num2 == Interlocked.CompareExchange(ref _cacheSize, num, num2))
			{
				return false;
			}
		}
		return true;
	}

	private void TriggerOvercapacityCompaction()
	{
		_logger.LogDebug("Overcapacity compaction triggered");
		ThreadPool.QueueUserWorkItem(delegate(object s)
		{
			OvercapacityCompaction((MemoryCache)s);
		}, this);
	}

	private static void OvercapacityCompaction(MemoryCache cache)
	{
		long num = Interlocked.Read(ref cache._cacheSize);
		cache._logger.LogDebug($"Overcapacity compaction executing. Current size {num}");
		double? num2 = (double?)cache._options.SizeLimit * (1.0 - cache._options.CompactionPercentage);
		if ((double)num > num2)
		{
			cache.Compact(num - (long)num2.Value, (CacheEntry entry) => entry.Size.Value);
		}
		cache._logger.LogDebug($"Overcapacity compaction executed. New size {Interlocked.Read(ref cache._cacheSize)}");
	}

	public void Compact(double percentage)
	{
		int num = (int)((double)_entries.Count * percentage);
		Compact(num, (CacheEntry _) => 1L);
	}

	private void Compact(long removalSizeTarget, Func<CacheEntry, long> computeEntrySize)
	{
		List<CacheEntry> list = new List<CacheEntry>();
		List<CacheEntry> list2 = new List<CacheEntry>();
		List<CacheEntry> list3 = new List<CacheEntry>();
		List<CacheEntry> list4 = new List<CacheEntry>();
		long removedSize = 0L;
		DateTimeOffset utcNow = _options.Clock.UtcNow;
		foreach (CacheEntry value in _entries.Values)
		{
			if (value.CheckExpired(utcNow))
			{
				list.Add(value);
				removedSize += computeEntrySize(value);
				continue;
			}
			switch (value.Priority)
			{
			case CacheItemPriority.Low:
				list2.Add(value);
				break;
			case CacheItemPriority.Normal:
				list3.Add(value);
				break;
			case CacheItemPriority.High:
				list4.Add(value);
				break;
			default:
				throw new NotSupportedException("Not implemented: " + value.Priority);
			case CacheItemPriority.NeverRemove:
				break;
			}
		}
		ExpirePriorityBucket(ref removedSize, removalSizeTarget, computeEntrySize, list, list2);
		ExpirePriorityBucket(ref removedSize, removalSizeTarget, computeEntrySize, list, list3);
		ExpirePriorityBucket(ref removedSize, removalSizeTarget, computeEntrySize, list, list4);
		foreach (CacheEntry item in list)
		{
			RemoveEntry(item);
		}
	}

	private void ExpirePriorityBucket(ref long removedSize, long removalSizeTarget, Func<CacheEntry, long> computeEntrySize, List<CacheEntry> entriesToRemove, List<CacheEntry> priorityEntries)
	{
		if (removalSizeTarget <= removedSize)
		{
			return;
		}
		foreach (CacheEntry item in priorityEntries.OrderBy((CacheEntry entry) => entry.LastAccessed))
		{
			item.SetExpired(EvictionReason.Capacity);
			entriesToRemove.Add(item);
			removedSize += computeEntrySize(item);
			if (removalSizeTarget <= removedSize)
			{
				break;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
			_disposed = true;
		}
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(typeof(MemoryCache).FullName);
		}
	}

	private static void ValidateCacheKey(object key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
	}
}
