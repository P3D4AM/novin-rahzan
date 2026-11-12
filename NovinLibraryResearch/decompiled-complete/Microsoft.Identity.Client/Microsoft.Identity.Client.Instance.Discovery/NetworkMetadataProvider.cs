using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.TelemetryCore;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Instance.Discovery;

internal class NetworkMetadataProvider : INetworkMetadataProvider
{
	private readonly IHttpManager _httpManager;

	private readonly IMatsTelemetryManager _telemetryManager;

	private readonly INetworkCacheMetadataProvider _networkCacheMetadataProvider;

	private readonly Uri _userProvidedInstanceDiscoveryUri;

	public NetworkMetadataProvider(IHttpManager httpManager, IMatsTelemetryManager telemetryManager, INetworkCacheMetadataProvider networkCacheMetadataProvider, Uri userProvidedInstanceDiscoveryUri = null)
	{
		_httpManager = httpManager ?? throw new ArgumentNullException("httpManager");
		_telemetryManager = telemetryManager ?? throw new ArgumentNullException("telemetryManager");
		_networkCacheMetadataProvider = networkCacheMetadataProvider ?? throw new ArgumentNullException("networkCacheMetadataProvider");
		_userProvidedInstanceDiscoveryUri = userProvidedInstanceDiscoveryUri;
	}

	public async Task<InstanceDiscoveryMetadataEntry> GetMetadataAsync(Uri authority, RequestContext requestContext)
	{
		ICoreLogger logger = requestContext.Logger;
		string environment = authority.Host;
		InstanceDiscoveryMetadataEntry metadata = _networkCacheMetadataProvider.GetMetadata(environment, logger);
		if (metadata != null)
		{
			logger.Verbose("[Instance Discovery] The network provider found an entry for " + environment);
			return metadata;
		}
		CacheInstanceDiscoveryMetadata(await FetchAllDiscoveryMetadataAsync(authority, requestContext).ConfigureAwait(continueOnCapturedContext: false));
		metadata = _networkCacheMetadataProvider.GetMetadata(environment, logger);
		logger.Verbose($"[Instance Discovery] After hitting the discovery endpoint, the network provider found an entry for {environment} ? {metadata != null}");
		return metadata;
	}

	private void CacheInstanceDiscoveryMetadata(InstanceDiscoveryResponse instanceDiscoveryResponse)
	{
		IEnumerable<InstanceDiscoveryMetadataEntry> metadata = instanceDiscoveryResponse.Metadata;
		foreach (InstanceDiscoveryMetadataEntry item in metadata ?? Enumerable.Empty<InstanceDiscoveryMetadataEntry>())
		{
			IEnumerable<string> aliases = item.Aliases;
			foreach (string item2 in aliases ?? Enumerable.Empty<string>())
			{
				_networkCacheMetadataProvider.AddMetadata(item2, item);
			}
		}
	}

	private async Task<InstanceDiscoveryResponse> FetchAllDiscoveryMetadataAsync(Uri authority, RequestContext requestContext)
	{
		return await SendInstanceDiscoveryRequestAsync(authority, requestContext).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<InstanceDiscoveryResponse> SendInstanceDiscoveryRequestAsync(Uri authority, RequestContext requestContext)
	{
		OAuth2Client oAuth2Client = new OAuth2Client(requestContext.Logger, _httpManager, _telemetryManager);
		oAuth2Client.AddQueryParameter("api-version", "1.1");
		oAuth2Client.AddQueryParameter("authorization_endpoint", BuildAuthorizeEndpoint(authority));
		Uri endPoint = ComputeHttpEndpoint(authority, requestContext);
		return await oAuth2Client.DiscoverAadInstanceAsync(endPoint, requestContext).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Uri ComputeHttpEndpoint(Uri authority, RequestContext requestContext)
	{
		if (_userProvidedInstanceDiscoveryUri != null)
		{
			return _userProvidedInstanceDiscoveryUri;
		}
		string text = (KnownMetadataProvider.IsKnownEnvironment(authority.Host) ? authority.Host : "login.microsoftonline.com");
		string httpsUriWithOptionalPort = UriBuilderExtensions.GetHttpsUriWithOptionalPort(string.Format(CultureInfo.InvariantCulture, "https://{0}/common/discovery/instance", text), authority.Port);
		requestContext.Logger.InfoPii("Fetching instance discovery from the network from host " + text + ". Endpoint " + httpsUriWithOptionalPort, "Fetching instance discovery from the network from host " + text);
		return new Uri(httpsUriWithOptionalPort);
	}

	private static string BuildAuthorizeEndpoint(Uri authority)
	{
		return UriBuilderExtensions.GetHttpsUriWithOptionalPort(authority.Host, GetTenant(authority), "oauth2/v2.0/authorize", authority.Port);
	}

	private static string GetTenant(Uri uri)
	{
		return uri.AbsolutePath.Split(new char[1] { '/' })[1];
	}
}
