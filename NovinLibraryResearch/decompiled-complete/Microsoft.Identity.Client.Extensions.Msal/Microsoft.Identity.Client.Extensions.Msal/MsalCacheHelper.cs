using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Extensions.Msal;

public class MsalCacheHelper
{
	public const string LinuxKeyRingDefaultCollection = "default";

	public const string LinuxKeyRingSessionCollection = "session";

	private static readonly Lazy<TraceSourceLogger> s_staticLogger = new Lazy<TraceSourceLogger>(() => new TraceSourceLogger(EnvUtils.GetNewTraceSource("MsalCacheHelperSingleton")));

	private readonly object _lockObject = new object();

	private readonly StorageCreationProperties _storageCreationProperties;

	private readonly TraceSourceLogger _logger;

	private HashSet<string> _knownAccountIds;

	internal readonly FileSystemWatcher _cacheWatcher;

	internal readonly HashSet<ITokenCache> _registeredCaches = new HashSet<ITokenCache>();

	internal CrossPlatLock CacheLock { get; private set; }

	internal MsalCacheStorage CacheStore { get; }

	public static string UserRootDirectory => SharedUtilities.GetUserRootDirectory();

	public event EventHandler<CacheChangedEventArgs> CacheChanged;

	private static async Task<HashSet<string>> GetAccountIdentifiersAsync(StorageCreationProperties storageCreationProperties, TraceSourceLogger logger)
	{
		HashSet<string> accountIdentifiers = new HashSet<string>();
		if (File.Exists(storageCreationProperties.CacheFilePath))
		{
			IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder.Create(storageCreationProperties.ClientId).Build();
			publicClientApplication.UserTokenCache.SetBeforeAccess(delegate(TokenCacheNotificationArgs args)
			{
				MsalCacheStorage msalCacheStorage = null;
				try
				{
					msalCacheStorage = MsalCacheStorage.Create(storageCreationProperties, s_staticLogger.Value.Source);
					byte[] msalV3State = msalCacheStorage.ReadData();
					args.TokenCache.DeserializeMsalV3(msalV3State);
				}
				catch (Exception ex)
				{
					logger.LogError("An error occured while reading the token cache: " + ex);
					logger.LogError("Deleting the token cache as it might be corrupt.");
					msalCacheStorage.Clear();
				}
			});
			foreach (IAccount item in await publicClientApplication.GetAccountsAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				accountIdentifiers.Add(item.HomeAccountId.Identifier);
			}
		}
		return accountIdentifiers;
	}

	private MsalCacheHelper(StorageCreationProperties storageCreationProperties, TraceSource logger, HashSet<string> knownAccountIds, FileSystemWatcher cacheWatcher)
	{
		_logger = ((logger == null) ? s_staticLogger.Value : new TraceSourceLogger(logger));
		_storageCreationProperties = storageCreationProperties;
		CacheStore = MsalCacheStorage.Create(_storageCreationProperties, _logger.Source);
		_knownAccountIds = knownAccountIds;
		_cacheWatcher = cacheWatcher;
		_cacheWatcher.Changed += OnCacheFileChangedAsync;
		_cacheWatcher.Deleted += OnCacheFileChangedAsync;
	}

	private async void OnCacheFileChangedAsync(object sender, FileSystemEventArgs args)
	{
		if (this.CacheChanged == null)
		{
			return;
		}
		try
		{
			IEnumerable<string> enumerable;
			IEnumerable<string> enumerable2;
			using (CreateCrossPlatLock(_storageCreationProperties))
			{
				HashSet<string> hashSet = await GetAccountIdentifiersAsync(_storageCreationProperties, _logger).ConfigureAwait(continueOnCapturedContext: false);
				IEnumerable<string> second = hashSet.Intersect(_knownAccountIds);
				enumerable = _knownAccountIds.Except(second);
				enumerable2 = hashSet.Except(second);
				_knownAccountIds = hashSet;
			}
			if (enumerable2.Any() || enumerable.Any())
			{
				this.CacheChanged(sender, new CacheChangedEventArgs(enumerable2, enumerable));
			}
		}
		catch (Exception arg)
		{
			_logger.LogWarning($"Exception within File Watcher : {arg}");
		}
	}

	internal MsalCacheHelper(ITokenCache userTokenCache, MsalCacheStorage store, TraceSource logger = null)
	{
		_logger = ((logger == null) ? s_staticLogger.Value : new TraceSourceLogger(logger));
		CacheStore = store;
		_storageCreationProperties = store.StorageCreationProperties;
		RegisterCache(userTokenCache);
	}

	public static async Task<MsalCacheHelper> CreateAsync(StorageCreationProperties storageCreationProperties, TraceSource logger = null)
	{
		if (storageCreationProperties == null)
		{
			throw new ArgumentNullException("storageCreationProperties");
		}
		using (CreateCrossPlatLock(storageCreationProperties))
		{
			TraceSourceLogger logger2 = ((logger == null) ? s_staticLogger.Value : new TraceSourceLogger(logger));
			HashSet<string> knownAccountIds = await GetAccountIdentifiersAsync(storageCreationProperties, logger2).ConfigureAwait(continueOnCapturedContext: false);
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(storageCreationProperties.CacheDirectory, storageCreationProperties.CacheFileName);
			MsalCacheHelper msalCacheHelper = new MsalCacheHelper(storageCreationProperties, logger, knownAccountIds, fileSystemWatcher);
			try
			{
				fileSystemWatcher.EnableRaisingEvents = true;
			}
			catch (PlatformNotSupportedException)
			{
				msalCacheHelper._logger.LogError("Cannot fire the CacheChanged event because the target framework does not support FileSystemWatcher. This is a known issue in Xamarin / Mono.");
			}
			return msalCacheHelper;
		}
	}

	public void RegisterCache(ITokenCache tokenCache)
	{
		if (tokenCache == null)
		{
			throw new ArgumentNullException("tokenCache");
		}
		lock (_lockObject)
		{
			_logger.LogInformation("Registering token cache with on disk storage");
			if (_registeredCaches.Contains(tokenCache))
			{
				_logger.LogWarning("Redundant registration of tokenCache in MsalCacheHelper, skipping further registration.");
				return;
			}
			tokenCache.SetBeforeAccess(BeforeAccessNotification);
			tokenCache.SetAfterAccess(AfterAccessNotification);
			_logger.LogInformation("Initializing msal cache");
		}
		_registeredCaches.Add(tokenCache);
		_logger.LogInformation("Done initializing");
	}

	public void UnregisterCache(ITokenCache tokenCache)
	{
		lock (_lockObject)
		{
			_logger.LogInformation("Unregistering token cache from on disk storage");
			if (_registeredCaches.Contains(tokenCache))
			{
				_registeredCaches.Remove(tokenCache);
				tokenCache.SetBeforeAccess(delegate
				{
				});
				tokenCache.SetAfterAccess(delegate
				{
				});
			}
			else
			{
				_logger.LogWarning("Attempting to unregister an already unregistered tokenCache in MsalCacheHelper");
			}
		}
	}

	public void Clear()
	{
		CacheStore.Clear();
	}

	public byte[] LoadUnencryptedTokenCache()
	{
		using (CreateCrossPlatLock(_storageCreationProperties))
		{
			return CacheStore.ReadData(ignoreExceptions: false);
		}
	}

	public void SaveUnencryptedTokenCache(byte[] tokenCache)
	{
		using (CreateCrossPlatLock(_storageCreationProperties))
		{
			CacheStore.WriteData(tokenCache, ignoreExceptions: false);
		}
	}

	private static CrossPlatLock CreateCrossPlatLock(StorageCreationProperties storageCreationProperties)
	{
		return new CrossPlatLock(storageCreationProperties.CacheFilePath + ".lockfile", storageCreationProperties.LockRetryDelay, storageCreationProperties.LockRetryCount);
	}

	internal void BeforeAccessNotification(TokenCacheNotificationArgs args)
	{
		_logger.LogInformation("Before access");
		_logger.LogInformation("Acquiring lock for token cache");
		CacheLock = CreateCrossPlatLock(_storageCreationProperties);
		_logger.LogInformation("Before access, the store has changed");
		byte[] array = CacheStore.ReadData();
		_logger.LogInformation($"Read '{array?.Length}' bytes from storage");
		lock (_lockObject)
		{
			try
			{
				_logger.LogInformation("Deserializing the store");
				args.TokenCache.DeserializeMsalV3(array, shouldClearExistingCache: true);
			}
			catch (Exception arg)
			{
				_logger.LogError(string.Format("An exception was encountered while deserializing the {0} : {1}", "MsalCacheHelper", arg));
				_logger.LogError("No data found in the store, clearing the cache in memory.");
				Clear();
				throw;
			}
		}
	}

	internal void AfterAccessNotification(TokenCacheNotificationArgs args)
	{
		try
		{
			_logger.LogInformation("After access");
			if (args.HasStateChanged)
			{
				_logger.LogInformation("After access, cache in memory HasChanged");
				try
				{
					_logger.LogInformation("Before Write Store");
					byte[] array = args.TokenCache.SerializeMsalV3();
					_logger.LogInformation($"Serializing '{array.Length}' bytes");
					CacheStore.WriteData(array);
					_logger.LogInformation("After write store");
					return;
				}
				catch (Exception arg)
				{
					_logger.LogError(string.Format("An exception was encountered while serializing the {0} : {1}", "MsalCacheHelper", arg));
					_logger.LogError("No data found in the store, clearing the cache in memory.");
					Clear();
					throw;
				}
			}
		}
		finally
		{
			ReleaseFileLock();
		}
	}

	private void ReleaseFileLock()
	{
		CrossPlatLock cacheLock = CacheLock;
		CacheLock = null;
		cacheLock?.Dispose();
		_logger.LogInformation("Released lock");
	}

	public void VerifyPersistence()
	{
		CacheStore.VerifyPersistence();
	}
}
