using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenByRefreshTokenParameterBuilder : AbstractClientAppBaseAcquireTokenParameterBuilder<AcquireTokenByRefreshTokenParameterBuilder>
{
	private AcquireTokenByRefreshTokenParameters Parameters { get; } = new AcquireTokenByRefreshTokenParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenByRefreshToken;

	internal AcquireTokenByRefreshTokenParameterBuilder(IClientApplicationBaseExecutor clientApplicationBaseExecutor)
		: base(clientApplicationBaseExecutor)
	{
	}

	internal static AcquireTokenByRefreshTokenParameterBuilder Create(IClientApplicationBaseExecutor clientApplicationBaseExecutor, IEnumerable<string> scopes, string refreshToken)
	{
		return new AcquireTokenByRefreshTokenParameterBuilder(clientApplicationBaseExecutor).WithScopes(scopes).WithRefreshToken(refreshToken);
	}

	internal AcquireTokenByRefreshTokenParameterBuilder WithRefreshToken(string refreshToken)
	{
		Parameters.RefreshToken = refreshToken;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.ClientApplicationBaseExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenByRefreshToken;
	}

	public AcquireTokenByRefreshTokenParameterBuilder WithSendX5C(bool withSendX5C)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSendX5C);
		Parameters.SendX5C = withSendX5C;
		return this;
	}
}
