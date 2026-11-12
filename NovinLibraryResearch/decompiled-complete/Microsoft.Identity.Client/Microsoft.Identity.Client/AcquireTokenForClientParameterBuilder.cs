using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenForClientParameterBuilder : AbstractConfidentialClientAcquireTokenParameterBuilder<AcquireTokenForClientParameterBuilder>
{
	private AcquireTokenForClientParameters Parameters { get; } = new AcquireTokenForClientParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenForClient;

	internal AcquireTokenForClientParameterBuilder(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor)
		: base(confidentialClientApplicationExecutor)
	{
	}

	internal static AcquireTokenForClientParameterBuilder Create(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor, IEnumerable<string> scopes)
	{
		return new AcquireTokenForClientParameterBuilder(confidentialClientApplicationExecutor).WithScopes(scopes);
	}

	public AcquireTokenForClientParameterBuilder WithForceRefresh(bool forceRefresh)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithForceRefresh, forceRefresh);
		Parameters.ForceRefresh = forceRefresh;
		return this;
	}

	public AcquireTokenForClientParameterBuilder WithSendX5C(bool withSendX5C)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSendX5C, withSendX5C);
		Parameters.SendX5C = withSendX5C;
		return this;
	}

	public AcquireTokenForClientParameterBuilder WithAzureRegion(bool autoDetectRegion)
	{
		if (!base.ServiceBundle.Config.ExperimentalFeaturesEnabled)
		{
			throw new MsalClientException("experimental_feature", MsalErrorMessage.ExperimentalFeature("WithAzureRegion"));
		}
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAzureRegion, autoDetectRegion);
		Parameters.AutoDetectRegion = autoDetectRegion;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.ConfidentialClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenForClient;
	}
}
