using System;
using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Threading;

namespace Microsoft.Data.SqlClient;

internal class EnclaveSessionCache
{
	private readonly MemoryCache enclaveMemoryCache = new MemoryCache("EnclaveMemoryCache", (NameValueCollection)null);

	private readonly object enclaveCacheLock = new object();

	private long _counter;

	private static int enclaveCacheTimeOutInHours = 8;

	internal SqlEnclaveSession GetEnclaveSession(EnclaveSessionParameters enclaveSessionParameters, out long counter)
	{
		string text = GenerateCacheKey(enclaveSessionParameters);
		SqlEnclaveSession result = ((ObjectCache)enclaveMemoryCache)[text] as SqlEnclaveSession;
		counter = Interlocked.Increment(ref _counter);
		return result;
	}

	internal void InvalidateSession(EnclaveSessionParameters enclaveSessionParameters, SqlEnclaveSession enclaveSessionToInvalidate)
	{
		string text = GenerateCacheKey(enclaveSessionParameters);
		lock (enclaveCacheLock)
		{
			long counter;
			SqlEnclaveSession enclaveSession = GetEnclaveSession(enclaveSessionParameters, out counter);
			if (enclaveSession != null && enclaveSession.SessionId == enclaveSessionToInvalidate.SessionId)
			{
				SqlEnclaveSession sqlEnclaveSession = ((ObjectCache)enclaveMemoryCache).Remove(text, (string)null) as SqlEnclaveSession;
				if (sqlEnclaveSession == null)
				{
					throw new InvalidOperationException(Strings.EnclaveSessionInvalidationFailed);
				}
			}
		}
	}

	internal SqlEnclaveSession CreateSession(EnclaveSessionParameters enclaveSessionParameters, byte[] sharedSecret, long sessionId, out long counter)
	{
		string text = GenerateCacheKey(enclaveSessionParameters);
		SqlEnclaveSession sqlEnclaveSession = null;
		lock (enclaveCacheLock)
		{
			sqlEnclaveSession = new SqlEnclaveSession(sharedSecret, sessionId);
			((ObjectCache)enclaveMemoryCache).Add(text, (object)sqlEnclaveSession, (DateTimeOffset)DateTime.UtcNow.AddHours(enclaveCacheTimeOutInHours), (string)null);
			counter = Interlocked.Increment(ref _counter);
			return sqlEnclaveSession;
		}
	}

	private string GenerateCacheKey(EnclaveSessionParameters enclaveSessionParameters)
	{
		return (enclaveSessionParameters.ServerName + "+" + enclaveSessionParameters.Database + enclaveSessionParameters.AttestationUrl).ToLowerInvariant();
	}
}
