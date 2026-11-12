using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class GetAuthorizationRequestUrlParameterBuilder : AbstractConfidentialClientAcquireTokenParameterBuilder<GetAuthorizationRequestUrlParameterBuilder>
{
	private GetAuthorizationRequestUrlParameters Parameters { get; } = new GetAuthorizationRequestUrlParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.GetAuthorizationRequestUrl;

	internal GetAuthorizationRequestUrlParameterBuilder(IConfidentialClientApplicationExecutor confidentialClientApplicationexecutor)
		: base(confidentialClientApplicationexecutor)
	{
	}

	internal static GetAuthorizationRequestUrlParameterBuilder Create(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor, IEnumerable<string> scopes)
	{
		return new GetAuthorizationRequestUrlParameterBuilder(confidentialClientApplicationExecutor).WithScopes(scopes);
	}

	public GetAuthorizationRequestUrlParameterBuilder WithRedirectUri(string redirectUri)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithRedirectUri);
		Parameters.RedirectUri = redirectUri;
		return this;
	}

	public GetAuthorizationRequestUrlParameterBuilder WithLoginHint(string loginHint)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithLoginHint);
		Parameters.LoginHint = loginHint;
		return this;
	}

	public GetAuthorizationRequestUrlParameterBuilder WithAccount(IAccount account)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAccount);
		Parameters.Account = account;
		return this;
	}

	public GetAuthorizationRequestUrlParameterBuilder WithExtraScopesToConsent(IEnumerable<string> extraScopesToConsent)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithExtraScopesToConsent);
		Parameters.ExtraScopesToConsent = extraScopesToConsent;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		throw new InvalidOperationException("This is a developer BUG.  This should never get executed.");
	}

	public new Task<Uri> ExecuteAsync(CancellationToken cancellationToken)
	{
		ValidateAndCalculateApiId();
		return base.ConfidentialClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	public new Task<Uri> ExecuteAsync()
	{
		return ExecuteAsync(CancellationToken.None);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.GetAuthorizationRequestUrl;
	}
}
