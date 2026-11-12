using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenSilentParameterBuilder : AbstractClientAppBaseAcquireTokenParameterBuilder<AcquireTokenSilentParameterBuilder>
{
	private AcquireTokenSilentParameters Parameters { get; } = new AcquireTokenSilentParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenSilent;

	internal AcquireTokenSilentParameterBuilder(IClientApplicationBaseExecutor clientApplicationBaseExecutor)
		: base(clientApplicationBaseExecutor)
	{
	}

	internal static AcquireTokenSilentParameterBuilder Create(IClientApplicationBaseExecutor clientApplicationBaseExecutor, IEnumerable<string> scopes, IAccount account)
	{
		return new AcquireTokenSilentParameterBuilder(clientApplicationBaseExecutor).WithScopes(scopes).WithAccount(account);
	}

	internal static AcquireTokenSilentParameterBuilder Create(IClientApplicationBaseExecutor clientApplicationBaseExecutor, IEnumerable<string> scopes, string loginHint)
	{
		return new AcquireTokenSilentParameterBuilder(clientApplicationBaseExecutor).WithScopes(scopes).WithLoginHint(loginHint);
	}

	private AcquireTokenSilentParameterBuilder WithAccount(IAccount account)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAccount);
		Parameters.Account = account;
		return this;
	}

	private AcquireTokenSilentParameterBuilder WithLoginHint(string loginHint)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithLoginHint);
		Parameters.LoginHint = loginHint;
		return this;
	}

	public AcquireTokenSilentParameterBuilder WithForceRefresh(bool forceRefresh)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithForceRefresh, forceRefresh);
		Parameters.ForceRefresh = forceRefresh;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.ClientApplicationBaseExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenSilent;
	}

	public AcquireTokenSilentParameterBuilder WithSendX5C(bool withSendX5C)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSendX5C);
		Parameters.SendX5C = withSendX5C;
		return this;
	}
}
