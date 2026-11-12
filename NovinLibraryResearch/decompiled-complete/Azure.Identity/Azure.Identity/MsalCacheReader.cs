using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Azure.Identity;

internal class MsalCacheReader
{
	private readonly string _cachePath;

	private readonly string _cacheLockPath;

	private readonly int _cacheRetryCount;

	private readonly TimeSpan _cacheRetryDelay;

	private DateTimeOffset _lastReadTime;

	public MsalCacheReader(ITokenCache cache, string cachePath, int cacheRetryCount, TimeSpan cacheRetryDelay)
	{
		_cachePath = cachePath;
		_cacheLockPath = cachePath + ".lockfile";
		_cacheRetryCount = cacheRetryCount;
		_cacheRetryDelay = cacheRetryDelay;
		cache.SetBeforeAccessAsync(OnBeforeAccessAsync);
	}

	private async Task OnBeforeAccessAsync(TokenCacheNotificationArgs args)
	{
		_ = 1;
		try
		{
			DateTime cacheTimestamp = File.GetLastWriteTimeUtc(_cachePath);
			if (!File.Exists(_cachePath) || !(_lastReadTime < cacheTimestamp))
			{
				return;
			}
			using (await SentinelFileLock.AcquireAsync(_cacheLockPath, _cacheRetryCount, _cacheRetryDelay).ConfigureAwait(continueOnCapturedContext: false))
			{
				byte[] array = await ReadCacheFromProtectedStorageAsync().ConfigureAwait(continueOnCapturedContext: false);
				_lastReadTime = cacheTimestamp;
				if (array != null)
				{
					args.TokenCache.DeserializeMsalV3(array);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private async Task<byte[]> ReadCacheFromProtectedStorageAsync()
	{
		using FileStream file = File.OpenRead(_cachePath);
		byte[] protectedBytes = new byte[file.Length];
		await file.ReadAsync(protectedBytes, 0, protectedBytes.Length).ConfigureAwait(continueOnCapturedContext: false);
		return ProtectedData.Unprotect(protectedBytes, (byte[])null, (DataProtectionScope)0);
	}
}
