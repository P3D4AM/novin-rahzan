using System;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace Microsoft.Data.ProviderBase;

internal sealed class DbConnectionPoolAuthenticationContext
{
	private const int STATUS_LOCKED = 1;

	private const int STATUS_UNLOCKED = 0;

	private readonly byte[] _accessToken;

	private readonly DateTime _expirationTime;

	private int _isUpdateInProgress;

	internal byte[] AccessToken => _accessToken;

	internal DateTime ExpirationTime => _expirationTime;

	internal DbConnectionPoolAuthenticationContext(byte[] accessToken, DateTime expirationTime)
	{
		_accessToken = accessToken;
		_expirationTime = expirationTime;
		_isUpdateInProgress = 0;
	}

	internal static DbConnectionPoolAuthenticationContext ChooseAuthenticationContextToUpdate(DbConnectionPoolAuthenticationContext context1, DbConnectionPoolAuthenticationContext context2)
	{
		if (!(context1.ExpirationTime > context2.ExpirationTime))
		{
			return context2;
		}
		return context1;
	}

	internal bool LockToUpdate()
	{
		int num = Interlocked.CompareExchange(ref _isUpdateInProgress, 1, 0);
		return num == 0;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal void ReleaseLockToUpdate()
	{
		int num = Interlocked.CompareExchange(ref _isUpdateInProgress, 0, 1);
	}
}
