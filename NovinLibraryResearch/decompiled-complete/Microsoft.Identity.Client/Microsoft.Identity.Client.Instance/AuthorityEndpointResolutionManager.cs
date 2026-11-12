using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Instance.Validation;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Instance;

internal class AuthorityEndpointResolutionManager : IAuthorityEndpointResolutionManager
{
	private class AuthorityEndpointCacheEntry
	{
		public AuthorityEndpoints Endpoints { get; }

		public HashSet<string> ValidForDomainsList { get; } = new HashSet<string>();


		public AuthorityEndpointCacheEntry(AuthorityEndpoints endpoints)
		{
			Endpoints = endpoints;
		}
	}

	private static readonly ConcurrentDictionary<string, AuthorityEndpointCacheEntry> s_endpointCacheEntries = new ConcurrentDictionary<string, AuthorityEndpointCacheEntry>();

	private readonly IServiceBundle _serviceBundle;

	public AuthorityEndpointResolutionManager(IServiceBundle serviceBundle, bool shouldClearCache = true)
	{
		_serviceBundle = serviceBundle;
		if (shouldClearCache)
		{
			s_endpointCacheEntries.Clear();
		}
	}

	public async Task<AuthorityEndpoints> ResolveEndpointsAsync(AuthorityInfo authorityInfo, string userPrincipalName, RequestContext requestContext)
	{
		if (TryGetCacheValue(authorityInfo, userPrincipalName, out var endpoints))
		{
			requestContext.Logger.Info("Resolving authority endpoints... Already resolved? - TRUE");
			return endpoints;
		}
		requestContext.Logger.Info("Resolving authority endpoints... Already resolved? - FALSE");
		await AuthorityValidatorFactory.Create(authorityInfo, _serviceBundle).ValidateAuthorityAsync(authorityInfo, requestContext).ConfigureAwait(continueOnCapturedContext: false);
		endpoints = Authority.CreateAuthority(authorityInfo).GetHardcodedEndpoints();
		Add(authorityInfo, userPrincipalName, endpoints);
		return endpoints;
	}

	private bool TryGetCacheValue(AuthorityInfo authorityInfo, string userPrincipalName, out AuthorityEndpoints endpoints)
	{
		endpoints = null;
		if (!s_endpointCacheEntries.TryGetValue(authorityInfo.CanonicalAuthority, out var value))
		{
			return false;
		}
		if (authorityInfo.AuthorityType != AuthorityType.Adfs)
		{
			endpoints = value.Endpoints;
			return true;
		}
		if (!string.IsNullOrEmpty(userPrincipalName) && !value.ValidForDomainsList.Contains(AdfsUpnHelper.GetDomainFromUpn(userPrincipalName)))
		{
			return false;
		}
		endpoints = value.Endpoints;
		return true;
	}

	private void Add(AuthorityInfo authorityInfo, string userPrincipalName, AuthorityEndpoints endpoints)
	{
		AuthorityEndpointCacheEntry authorityEndpointCacheEntry = new AuthorityEndpointCacheEntry(endpoints);
		if (authorityInfo.AuthorityType == AuthorityType.Adfs)
		{
			if (s_endpointCacheEntries.TryGetValue(authorityInfo.CanonicalAuthority, out var value))
			{
				foreach (string validForDomains in value.ValidForDomainsList)
				{
					authorityEndpointCacheEntry.ValidForDomainsList.Add(validForDomains);
				}
			}
			if (!string.IsNullOrEmpty(userPrincipalName))
			{
				authorityEndpointCacheEntry.ValidForDomainsList.Add(AdfsUpnHelper.GetDomainFromUpn(userPrincipalName));
			}
		}
		s_endpointCacheEntries.TryAdd(authorityInfo.CanonicalAuthority, authorityEndpointCacheEntry);
	}
}
