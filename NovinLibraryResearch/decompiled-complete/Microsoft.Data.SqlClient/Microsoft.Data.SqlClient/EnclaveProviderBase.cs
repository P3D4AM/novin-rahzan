using System;
using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Threading;

namespace Microsoft.Data.SqlClient;

internal abstract class EnclaveProviderBase : SqlColumnEncryptionEnclaveProvider
{
	private const int NonceSize = 256;

	private const int ThreadRetryCacheTimeoutInMinutes = 10;

	private const int LockTimeoutMaxInMilliseconds = 15000;

	private static readonly EnclaveSessionCache SessionCache = new EnclaveSessionCache();

	private static AutoResetEvent sessionLockEvent = new AutoResetEvent(initialState: true);

	private static int lockTimeoutInMilliseconds = 15000;

	private static bool isSessionLockAcquired = false;

	private static readonly object lockUpdateSessionLock = new object();

	protected static readonly MemoryCache ThreadRetryCache = new MemoryCache("ThreadRetryCache", (NameValueCollection)null);

	protected void GetEnclaveSessionHelper(EnclaveSessionParameters enclaveSessionParameters, bool shouldGenerateNonce, out SqlEnclaveSession sqlEnclaveSession, out long counter, out byte[] customData, out int customDataLength)
	{
		customData = null;
		customDataLength = 0;
		sqlEnclaveSession = SessionCache.GetEnclaveSession(enclaveSessionParameters, out counter);
		if (sqlEnclaveSession != null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		string text = ((ObjectCache)ThreadRetryCache)[Thread.CurrentThread.ManagedThreadId.ToString()] as string;
		if (!string.IsNullOrEmpty(text))
		{
			flag2 = true;
		}
		else
		{
			flag = sessionLockEvent.WaitOne(lockTimeoutInMilliseconds);
			if (flag)
			{
				lock (lockUpdateSessionLock)
				{
					isSessionLockAcquired = true;
				}
			}
		}
		if (flag || flag2)
		{
			sqlEnclaveSession = SessionCache.GetEnclaveSession(enclaveSessionParameters, out counter);
			if (sqlEnclaveSession != null && !flag2)
			{
				lock (lockUpdateSessionLock)
				{
					isSessionLockAcquired = false;
					sessionLockEvent.Set();
				}
			}
		}
		else
		{
			Interlocked.Exchange(ref lockTimeoutInMilliseconds, 0);
		}
		if (sqlEnclaveSession != null)
		{
			return;
		}
		if (shouldGenerateNonce)
		{
			using RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider();
			byte[] array = new byte[256];
			randomNumberGenerator.GetBytes(array);
			customData = array;
			customDataLength = array.Length;
		}
		if (!flag2)
		{
			text = Thread.CurrentThread.ManagedThreadId.ToString();
		}
		((ObjectCache)ThreadRetryCache).Set(Thread.CurrentThread.ManagedThreadId.ToString(), (object)text, (DateTimeOffset)DateTime.UtcNow.AddMinutes(10.0), (string)null);
	}

	protected void UpdateEnclaveSessionLockStatus(SqlEnclaveSession sqlEnclaveSession)
	{
		if (sqlEnclaveSession == null || !isSessionLockAcquired)
		{
			return;
		}
		lock (lockUpdateSessionLock)
		{
			if (isSessionLockAcquired)
			{
				isSessionLockAcquired = false;
				Interlocked.Exchange(ref lockTimeoutInMilliseconds, 15000);
				sessionLockEvent.Set();
			}
		}
	}

	protected void InvalidateEnclaveSessionHelper(EnclaveSessionParameters enclaveSessionParameters, SqlEnclaveSession enclaveSessionToInvalidate)
	{
		SessionCache.InvalidateSession(enclaveSessionParameters, enclaveSessionToInvalidate);
	}

	protected SqlEnclaveSession GetEnclaveSessionFromCache(EnclaveSessionParameters enclaveSessionParameters, out long counter)
	{
		return SessionCache.GetEnclaveSession(enclaveSessionParameters, out counter);
	}

	protected SqlEnclaveSession AddEnclaveSessionToCache(EnclaveSessionParameters enclaveSessionParameters, byte[] sharedSecret, long sessionId, out long counter)
	{
		return SessionCache.CreateSession(enclaveSessionParameters, sharedSecret, sessionId, out counter);
	}
}
