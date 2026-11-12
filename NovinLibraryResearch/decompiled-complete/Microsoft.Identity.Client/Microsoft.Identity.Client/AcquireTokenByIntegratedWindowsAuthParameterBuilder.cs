using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenByIntegratedWindowsAuthParameterBuilder : AbstractPublicClientAcquireTokenParameterBuilder<AcquireTokenByIntegratedWindowsAuthParameterBuilder>
{
	private AcquireTokenByIntegratedWindowsAuthParameters Parameters { get; } = new AcquireTokenByIntegratedWindowsAuthParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenByIntegratedWindowsAuth;

	internal AcquireTokenByIntegratedWindowsAuthParameterBuilder(IPublicClientApplicationExecutor publicClientApplicationExecutor)
		: base(publicClientApplicationExecutor)
	{
	}

	internal static AcquireTokenByIntegratedWindowsAuthParameterBuilder Create(IPublicClientApplicationExecutor publicClientApplicationExecutor, IEnumerable<string> scopes)
	{
		return new AcquireTokenByIntegratedWindowsAuthParameterBuilder(publicClientApplicationExecutor).WithScopes(scopes);
	}

	public AcquireTokenByIntegratedWindowsAuthParameterBuilder WithUsername(string username)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithUsername);
		Parameters.Username = username;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.PublicClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenByIntegratedWindowsAuth;
	}
}
