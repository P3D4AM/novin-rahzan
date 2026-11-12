using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.Internal.Requests.Silent;

internal class SilentRequest : RequestBase
{
	private readonly AcquireTokenSilentParameters _silentParameters;

	private readonly ISilentAuthRequestStrategy _clientStrategy;

	private readonly Lazy<ISilentAuthRequestStrategy> _brokerStrategyLazy;

	private readonly ICoreLogger _logger;

	public SilentRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenSilentParameters silentParameters, ISilentAuthRequestStrategy clientStrategyOverride = null, ISilentAuthRequestStrategy brokerStrategyOverride = null)
		: base(serviceBundle, authenticationRequestParameters, silentParameters)
	{
		SilentRequest request = this;
		_silentParameters = silentParameters;
		_brokerStrategyLazy = new Lazy<ISilentAuthRequestStrategy>(() => brokerStrategyOverride ?? new BrokerSilentStrategy(request, serviceBundle, authenticationRequestParameters, silentParameters, serviceBundle.PlatformProxy.CreateBroker(null)));
		_clientStrategy = clientStrategyOverride ?? new CacheSilentStrategy(this, serviceBundle, authenticationRequestParameters, silentParameters);
		_logger = authenticationRequestParameters.RequestContext.Logger;
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		await UpdateRequestWithAccountAsync().ConfigureAwait(continueOnCapturedContext: false);
		bool isBrokerConfigured = base.AuthenticationRequestParameters.IsBrokerConfigured && base.ServiceBundle.PlatformProxy.CanBrokerSupportSilentAuth();
		AuthenticationResult result = default(AuthenticationResult);
		object obj;
		int num;
		try
		{
			if (base.AuthenticationRequestParameters.Account != null)
			{
				_logger.Verbose("Attempting to acquire token using using local cache...");
				result = await _clientStrategy.ExecuteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				return result;
			}
			if (!isBrokerConfigured)
			{
				_logger.Verbose("No account passed to AcquireTokenSilent");
				throw new MsalUiRequiredException("user_null", "No account or login hint was passed to the AcquireTokenSilent call.", null, UiRequiredExceptionClassification.AcquireTokenSilentFailed);
			}
			_logger.Verbose("No account passed to AcquireTokenSilent. Only the Windows broker (WAM) may be able to log in user with a default account...");
			result = await _brokerStrategyLazy.Value.ExecuteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return result;
		}
		catch (MsalException ex)
		{
			obj = ex;
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		MsalException ex2 = (MsalException)obj;
		_logger.Verbose("Token cache could not satisfy silent request");
		if (isBrokerConfigured && ShouldTryWithBrokerError(ex2.ErrorCode))
		{
			_logger.Info("Attempting to use broker instead");
			return await _brokerStrategyLazy.Value.ExecuteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		ExceptionDispatchInfo.Capture((obj as Exception) ?? throw obj).Throw();
		return result;
	}

	private static bool ShouldTryWithBrokerError(string errorCode)
	{
		if (!string.Equals(errorCode, "invalid_grant", StringComparison.OrdinalIgnoreCase) && !string.Equals(errorCode, "interaction_required", StringComparison.OrdinalIgnoreCase) && !string.Equals(errorCode, "no_tokens_found", StringComparison.OrdinalIgnoreCase))
		{
			return string.Equals(errorCode, "no_account_for_login_hint", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	protected override void EnrichTelemetryApiEvent(ApiEvent apiEvent)
	{
		if (_silentParameters.LoginHint != null)
		{
			apiEvent.LoginHint = _silentParameters.LoginHint;
		}
	}

	internal new async Task<AuthenticationResult> CacheTokenResponseAndCreateAuthenticationResultAsync(MsalTokenResponse response)
	{
		return await base.CacheTokenResponseAndCreateAuthenticationResultAsync(response).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal async Task<AuthenticationResult> ExecuteTestAsync(CancellationToken cancellationToken)
	{
		return await ExecuteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task UpdateRequestWithAccountAsync()
	{
		IAccount account = await GetAccountFromParamsOrLoginHintAsync(_silentParameters.Account, _silentParameters.LoginHint).ConfigureAwait(continueOnCapturedContext: false);
		base.AuthenticationRequestParameters.Account = account;
		base.AuthenticationRequestParameters.Authority = Authority.CreateAuthorityForRequest(base.ServiceBundle.Config.AuthorityInfo, base.AuthenticationRequestParameters.AuthorityOverride, account?.HomeAccountId?.TenantId);
	}

	private async Task<IAccount> GetSingleAccountForLoginHintAsync(string loginHint)
	{
		if (!string.IsNullOrEmpty(loginHint))
		{
			List<IAccount> list = (await base.CacheManager.GetAccountsAsync().ConfigureAwait(continueOnCapturedContext: false)).Where((IAccount a) => !string.IsNullOrWhiteSpace(a.Username) && a.Username.Equals(loginHint, StringComparison.OrdinalIgnoreCase)).ToList();
			if (((IReadOnlyCollection<IAccount>)list).Count == 0)
			{
				throw new MsalUiRequiredException("no_account_for_login_hint", "You are trying to acquire a token silently using a login hint. No account was found in the token cache having this login hint.", null, UiRequiredExceptionClassification.AcquireTokenSilentFailed);
			}
			if (((IReadOnlyCollection<IAccount>)list).Count > 1)
			{
				throw new MsalUiRequiredException("multiple_accounts_for_login_hint", "You are trying to acquire a token silently using a login hint. Multiple accounts were found in the token cache having this login hint. Please choose an account manually an pass it in to AcquireTokenSilently.", null, UiRequiredExceptionClassification.AcquireTokenSilentFailed);
			}
			return ((IReadOnlyList<IAccount>)list)[0];
		}
		return null;
	}

	private async Task<IAccount> GetAccountFromParamsOrLoginHintAsync(IAccount account, string loginHint)
	{
		if (account != null)
		{
			return account;
		}
		return await GetSingleAccountForLoginHintAsync(loginHint).ConfigureAwait(continueOnCapturedContext: false);
	}
}
