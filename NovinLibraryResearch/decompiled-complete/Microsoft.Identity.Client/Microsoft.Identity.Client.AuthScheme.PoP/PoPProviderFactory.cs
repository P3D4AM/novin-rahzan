using System;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.AuthScheme.PoP;

internal class PoPProviderFactory
{
	private static InMemoryCryptoProvider s_currentProvider;

	private static DateTime s_providerExpiration;

	private static readonly TimeSpan s_expirationTimespan = TimeSpan.FromHours(8.0);

	private static object s_lock = new object();

	public static InMemoryCryptoProvider GetOrCreateProvider(ITimeService timeService = null)
	{
		if (timeService == null)
		{
			timeService = new TimeService();
		}
		lock (s_lock)
		{
			if (s_currentProvider != null && s_providerExpiration > timeService.GetUtcNow())
			{
				return s_currentProvider;
			}
			s_currentProvider = new InMemoryCryptoProvider();
			s_providerExpiration = timeService.GetUtcNow() + s_expirationTimespan;
			return s_currentProvider;
		}
	}
}
