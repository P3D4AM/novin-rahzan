using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.AuthScheme;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class AuthenticationRequestParameters
{
	private readonly IServiceBundle _serviceBundle;

	private readonly AcquireTokenCommonParameters _commonParameters;

	private string _loginHint;

	public ApiTelemetryId ApiTelemId => _commonParameters.ApiTelemId;

	public ApiEvent.ApiIds ApiId => _commonParameters.ApiId;

	public RequestContext RequestContext { get; }

	public Authority Authority { get; set; }

	public Authority UserConfiguredAuthority { get; set; }

	public AuthorityInfo AuthorityInfo => Authority.AuthorityInfo;

	public AuthorityEndpoints Endpoints { get; set; }

	public Authority TenantUpdatedCanonicalAuthority { get; set; }

	public ICacheSessionManager CacheSessionManager { get; }

	public HashSet<string> Scope { get; }

	public bool HasScopes
	{
		get
		{
			if (Scope != null)
			{
				return Scope.Count > 0;
			}
			return false;
		}
	}

	public string ClientId { get; }

	public Uri RedirectUri { get; set; }

	public IDictionary<string, string> ExtraQueryParameters { get; }

	public string ClaimsAndClientCapabilities { get; private set; }

	public Guid CorrelationId => _commonParameters.CorrelationId;

	public string Claims => _commonParameters.Claims;

	public AuthorityInfo AuthorityOverride => _commonParameters.AuthorityOverride;

	internal bool IsBrokerConfigured { get; set; }

	public IAuthenticationScheme AuthenticationScheme => _commonParameters.AuthenticationScheme;

	public ClientCredentialWrapper ClientCredential { get; set; }

	public bool SendX5C { get; set; }

	public string LoginHint
	{
		get
		{
			if (string.IsNullOrEmpty(_loginHint) && Account != null)
			{
				return Account.Username;
			}
			return _loginHint;
		}
		set
		{
			_loginHint = value;
		}
	}

	public IAccount Account { get; set; }

	public string HomeAccountId { get; }

	public bool IsClientCredentialRequest => ApiId == ApiEvent.ApiIds.AcquireTokenForClient;

	public bool IsConfidentialClient => ClientCredential != null;

	public UserAssertion UserAssertion { get; set; }

	public AuthenticationRequestParameters(IServiceBundle serviceBundle, ITokenCacheInternal tokenCache, AcquireTokenCommonParameters commonParameters, RequestContext requestContext, string homeAccountId = null)
	{
		_serviceBundle = serviceBundle;
		_commonParameters = commonParameters;
		Authority = Authority.CreateAuthorityForRequest(serviceBundle.Config.AuthorityInfo, commonParameters.AuthorityOverride);
		UserConfiguredAuthority = Authority;
		ClientId = serviceBundle.Config.ClientId;
		CacheSessionManager = new CacheSessionManager(tokenCache, this);
		Scope = ScopeHelper.CreateScopeSet(commonParameters.Scopes);
		RedirectUri = new Uri(serviceBundle.Config.RedirectUri);
		RequestContext = requestContext;
		IsBrokerConfigured = serviceBundle.Config.IsBrokerEnabled;
		ExtraQueryParameters = serviceBundle.Config.ExtraQueryParameters ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		if (commonParameters.ExtraQueryParameters != null)
		{
			foreach (KeyValuePair<string, string> extraQueryParameter in commonParameters.ExtraQueryParameters)
			{
				ExtraQueryParameters[extraQueryParameter.Key] = extraQueryParameter.Value;
			}
		}
		ClaimsAndClientCapabilities = ClaimsHelper.GetMergedClaimsAndClientCapabilities(_commonParameters.Claims, _serviceBundle.Config.ClientCapabilities);
		HomeAccountId = homeAccountId;
	}

	public IEnumerable<KeyValuePair<string, string>> GetApiTelemetryFeatures()
	{
		return _commonParameters.GetApiTelemetryFeatures();
	}

	public void LogParameters()
	{
		ICoreLogger logger = RequestContext.Logger;
		StringBuilder stringBuilder = new StringBuilder(Environment.NewLine + "=== Request Data ===" + Environment.NewLine + "Authority Provided? - " + (Authority != null) + Environment.NewLine);
		stringBuilder.AppendLine("Client Id - " + ClientId);
		stringBuilder.AppendLine("Scopes - " + Scope?.AsSingleString());
		stringBuilder.AppendLine("Redirect Uri - " + RedirectUri?.OriginalString);
		stringBuilder.AppendLine("Extra Query Params Keys (space separated) - " + ExtraQueryParameters.Keys.AsSingleString());
		stringBuilder.AppendLine("ClaimsAndClientCapabilities - " + ClaimsAndClientCapabilities);
		stringBuilder.AppendLine("Authority - " + AuthorityInfo?.CanonicalAuthority);
		stringBuilder.AppendLine("ApiId - " + ApiId);
		stringBuilder.AppendLine("IsConfidentialClient - " + IsConfidentialClient);
		stringBuilder.AppendLine("SendX5C - " + SendX5C);
		stringBuilder.AppendLine("LoginHint - " + LoginHint);
		stringBuilder.AppendLine("IsBrokerConfigured - " + IsBrokerConfigured);
		stringBuilder.AppendLine("HomeAccountId - " + HomeAccountId);
		stringBuilder.AppendLine("CorrelationId - " + CorrelationId);
		string messageWithPii = stringBuilder.ToString();
		stringBuilder = new StringBuilder(Environment.NewLine + "=== Request Data ===" + Environment.NewLine + "Authority Provided? - " + (Authority != null) + Environment.NewLine);
		stringBuilder.AppendLine("Scopes - " + Scope?.AsSingleString());
		stringBuilder.AppendLine("Extra Query Params Keys (space separated) - " + ExtraQueryParameters.Keys.AsSingleString());
		stringBuilder.AppendLine("ApiId - " + ApiId);
		stringBuilder.AppendLine("IsConfidentialClient - " + IsConfidentialClient);
		stringBuilder.AppendLine("SendX5C - " + SendX5C);
		stringBuilder.AppendLine("LoginHint ? " + !string.IsNullOrEmpty(LoginHint));
		stringBuilder.AppendLine("IsBrokerConfigured - " + IsBrokerConfigured);
		stringBuilder.AppendLine("HomeAccountId - " + !string.IsNullOrEmpty(HomeAccountId));
		stringBuilder.AppendLine("CorrelationId - " + CorrelationId);
		logger.InfoPii(messageWithPii, stringBuilder.ToString());
	}
}
