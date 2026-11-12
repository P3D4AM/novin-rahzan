using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal class SingletonThrottlingManager : IThrottlingProvider
{
	private static readonly Lazy<SingletonThrottlingManager> lazyPrivateCtor = new Lazy<SingletonThrottlingManager>(() => new SingletonThrottlingManager());

	public IEnumerable<IThrottlingProvider> ThrottlingProviders { get; }

	private SingletonThrottlingManager()
	{
		ThrottlingProviders = new List<IThrottlingProvider>
		{
			new RetryAfterProvider(),
			new HttpStatusProvider(),
			new UiRequiredProvider()
		};
	}

	public static SingletonThrottlingManager GetInstance()
	{
		return lazyPrivateCtor.Value;
	}

	public void RecordException(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams, MsalServiceException ex)
	{
		if (ex is MsalThrottledServiceException)
		{
			return;
		}
		foreach (IThrottlingProvider throttlingProvider in ThrottlingProviders)
		{
			throttlingProvider.RecordException(requestParams, bodyParams, ex);
		}
	}

	public void TryThrottle(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams)
	{
		foreach (IThrottlingProvider throttlingProvider in ThrottlingProviders)
		{
			throttlingProvider.TryThrottle(requestParams, bodyParams);
		}
	}

	public void ResetCache()
	{
		foreach (IThrottlingProvider throttlingProvider in ThrottlingProviders)
		{
			throttlingProvider.ResetCache();
		}
	}
}
