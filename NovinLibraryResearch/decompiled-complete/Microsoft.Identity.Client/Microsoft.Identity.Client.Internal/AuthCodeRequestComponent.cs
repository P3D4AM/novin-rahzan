using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.TelemetryCore.Internal;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.UI;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Internal;

internal class AuthCodeRequestComponent : IAuthCodeRequestComponent
{
	private readonly AuthenticationRequestParameters _requestParams;

	private readonly AcquireTokenInteractiveParameters _interactiveParameters;

	private readonly IServiceBundle _serviceBundle;

	public AuthCodeRequestComponent(AuthenticationRequestParameters requestParams, AcquireTokenInteractiveParameters interactiveParameters)
	{
		_requestParams = requestParams ?? throw new ArgumentNullException("requestParams");
		_interactiveParameters = interactiveParameters ?? throw new ArgumentNullException("requestParams");
		_serviceBundle = _requestParams.RequestContext.ServiceBundle;
	}

	public async Task<Tuple<string, string>> FetchAuthCodeAndPkceVerifierAsync(CancellationToken cancellationToken)
	{
		IWebUI webUi = CreateWebAuthenticationDialog();
		return await FetchAuthCodeAndPkceInternalAsync(webUi, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public Uri GetAuthorizationUriWithoutPkce()
	{
		return CreateAuthorizationUri().Item1;
	}

	private async Task<Tuple<string, string>> FetchAuthCodeAndPkceInternalAsync(IWebUI webUi, CancellationToken cancellationToken)
	{
		RedirectUriHelper.Validate(_requestParams.RedirectUri);
		_requestParams.RedirectUri = webUi.UpdateRedirectUri(_requestParams.RedirectUri);
		Tuple<Uri, string, string> tuple = CreateAuthorizationUri(addPkceAndState: true);
		Uri item = tuple.Item1;
		string state = tuple.Item2;
		string codeVerifier = tuple.Item3;
		UiEvent uiEvent = new UiEvent(_requestParams.RequestContext.CorrelationId.AsMatsCorrelationId());
		using (_requestParams.RequestContext.CreateTelemetryHelper(uiEvent))
		{
			AuthorizationResult authorizationResult = await webUi.AcquireAuthorizationAsync(item, _requestParams.RedirectUri, _requestParams.RequestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			uiEvent.UserCancelled = authorizationResult.Status == AuthorizationStatus.UserCancel;
			uiEvent.AccessDenied = authorizationResult.Status == AuthorizationStatus.ProtocolError;
			VerifyAuthorizationResult(authorizationResult, state);
			return new Tuple<string, string>(authorizationResult.Code, codeVerifier);
		}
	}

	private Tuple<Uri, string, string> CreateAuthorizationUri(bool addPkceAndState = false)
	{
		IDictionary<string, string> dictionary = CreateAuthorizationRequestParameters();
		string text = null;
		string item = null;
		if (addPkceAndState)
		{
			text = _serviceBundle.PlatformProxy.CryptographyManager.GenerateCodeVerifier();
			string value = _serviceBundle.PlatformProxy.CryptographyManager.CreateBase64UrlEncodedSha256Hash(text);
			dictionary["code_challenge"] = value;
			dictionary["code_challenge_method"] = "S256";
			item = (dictionary["state"] = Guid.NewGuid().ToString() + Guid.NewGuid());
		}
		if (_interactiveParameters.Account != null)
		{
			if (!string.IsNullOrEmpty(_interactiveParameters.Account.Username))
			{
				dictionary["login_hint"] = _interactiveParameters.Account.Username;
			}
			if (_interactiveParameters.Account?.HomeAccountId?.ObjectId != null)
			{
				dictionary["login_req"] = _interactiveParameters.Account.HomeAccountId.ObjectId;
			}
			if (!string.IsNullOrEmpty(_interactiveParameters.Account?.HomeAccountId?.TenantId))
			{
				dictionary["domain_req"] = _interactiveParameters.Account.HomeAccountId.TenantId;
			}
		}
		CheckForDuplicateQueryParameters(_requestParams.ExtraQueryParameters, dictionary);
		string queryParams = dictionary.ToQueryParameter();
		UriBuilder uriBuilder = new UriBuilder(new Uri(_requestParams.Endpoints.AuthorizationEndpoint));
		uriBuilder.AppendQueryParameters(queryParams);
		return new Tuple<Uri, string, string>(uriBuilder.Uri, item, text);
	}

	private Dictionary<string, string> CreateAuthorizationRequestParameters(Uri redirectUriOverride = null)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		if (!_interactiveParameters.ExtraScopesToConsent.IsNullOrEmpty())
		{
			hashSet = ScopeHelper.CreateScopeSet(_interactiveParameters.ExtraScopesToConsent);
		}
		if (hashSet.Contains(_requestParams.ClientId))
		{
			throw new ArgumentException("API does not accept client id as a user-provided scope");
		}
		HashSet<string> msalScopes = ScopeHelper.GetMsalScopes(new HashSet<string>(_requestParams.Scope.Concat(hashSet)));
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			["scope"] = msalScopes.AsSingleString(),
			["response_type"] = "code",
			["client_id"] = _requestParams.ClientId,
			["redirect_uri"] = redirectUriOverride?.OriginalString ?? _requestParams.RedirectUri.OriginalString
		};
		if (!string.IsNullOrWhiteSpace(_requestParams.ClaimsAndClientCapabilities))
		{
			dictionary["claims"] = _requestParams.ClaimsAndClientCapabilities;
		}
		if (!string.IsNullOrWhiteSpace(_interactiveParameters.LoginHint))
		{
			dictionary["login_hint"] = _interactiveParameters.LoginHint;
		}
		if (_requestParams.RequestContext.CorrelationId != Guid.Empty)
		{
			dictionary["client-request-id"] = _requestParams.RequestContext.CorrelationId.ToString();
		}
		foreach (KeyValuePair<string, string> msalIdParameter in MsalIdHelper.GetMsalIdParameters(_requestParams.RequestContext.Logger))
		{
			dictionary[msalIdParameter.Key] = msalIdParameter.Value;
		}
		if (_interactiveParameters.Prompt == Prompt.NotSpecified)
		{
			dictionary["prompt"] = Prompt.SelectAccount.PromptValue;
		}
		else if (_interactiveParameters.Prompt.PromptValue != Prompt.NoPrompt.PromptValue)
		{
			dictionary["prompt"] = _interactiveParameters.Prompt.PromptValue;
		}
		return dictionary;
	}

	private static void CheckForDuplicateQueryParameters(IDictionary<string, string> queryParamsDictionary, IDictionary<string, string> requestParameters)
	{
		foreach (KeyValuePair<string, string> item in queryParamsDictionary)
		{
			if (requestParameters.ContainsKey(item.Key))
			{
				throw new MsalClientException("duplicate_query_parameter", string.Format(CultureInfo.InvariantCulture, "Duplicate query parameter '{0}' in extraQueryParameters", item.Key));
			}
			requestParameters[item.Key] = item.Value;
		}
	}

	private void VerifyAuthorizationResult(AuthorizationResult authorizationResult, string originalState)
	{
		if (authorizationResult.Status == AuthorizationStatus.Success && originalState != null && !originalState.Equals(authorizationResult.State, StringComparison.OrdinalIgnoreCase))
		{
			throw new MsalClientException("state_mismatch", string.Format(CultureInfo.InvariantCulture, "Returned state({0}) from authorize endpoint is not the same as the one sent({1}). See https://aka.ms/msal-statemismatcherror for more details.", authorizationResult.State, originalState));
		}
		if (authorizationResult.Error == "login_required")
		{
			throw new MsalUiRequiredException("no_prompt_failed", "One of two conditions was encountered: 1. The Prompt.Never flag was passed, but the constraint could not be honored, because user interaction was required. 2. An error occurred during a silent web authentication that prevented the http authentication flow from completing in a short enough time frame", null, UiRequiredExceptionClassification.PromptNeverFailed);
		}
		if (authorizationResult.Status == AuthorizationStatus.UserCancel)
		{
			_requestParams.RequestContext.Logger.Info("Authorization result status returned user cancelled authentication. ");
			throw new MsalClientException(authorizationResult.Error, authorizationResult.ErrorDescription ?? "User canceled authentication.");
		}
		if (authorizationResult.Status != 0)
		{
			_requestParams.RequestContext.Logger.ErrorPii("Authorization result was not successful. See error message for more details. " + authorizationResult.ErrorDescription, "Authorization result was not successful. See error message for more details. ");
			throw new MsalServiceException(authorizationResult.Error, (!string.IsNullOrEmpty(authorizationResult.ErrorDescription)) ? authorizationResult.ErrorDescription : "Unknown error");
		}
	}

	private IWebUI CreateWebAuthenticationDialog()
	{
		if (_interactiveParameters.CustomWebUi != null)
		{
			return new CustomWebUiHandler(_interactiveParameters.CustomWebUi);
		}
		CoreUIParent uiParent = _interactiveParameters.UiParent;
		uiParent.UseEmbeddedWebview = GetUseEmbeddedWebview(_interactiveParameters.UseEmbeddedWebView, _serviceBundle.PlatformProxy.UseEmbeddedWebViewDefault);
		uiParent.UseHiddenBrowser = _interactiveParameters.Prompt.Equals(Prompt.Never);
		return _serviceBundle.PlatformProxy.GetWebUiFactory().CreateAuthenticationDialog(uiParent, _requestParams.RequestContext);
	}

	private static bool GetUseEmbeddedWebview(WebViewPreference userPreference, bool defaultValue)
	{
		return userPreference switch
		{
			WebViewPreference.NotSpecified => defaultValue, 
			WebViewPreference.Embedded => true, 
			WebViewPreference.System => false, 
			_ => throw new NotImplementedException("Unknown option"), 
		};
	}
}
