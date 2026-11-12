using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenOnBehalfOfParameterBuilder : AbstractConfidentialClientAcquireTokenParameterBuilder<AcquireTokenOnBehalfOfParameterBuilder>
{
	private AcquireTokenOnBehalfOfParameters Parameters { get; } = new AcquireTokenOnBehalfOfParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenOnBehalfOf;

	internal AcquireTokenOnBehalfOfParameterBuilder(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor)
		: base(confidentialClientApplicationExecutor)
	{
	}

	internal static AcquireTokenOnBehalfOfParameterBuilder Create(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor, IEnumerable<string> scopes, UserAssertion userAssertion)
	{
		return new AcquireTokenOnBehalfOfParameterBuilder(confidentialClientApplicationExecutor).WithScopes(scopes).WithUserAssertion(userAssertion);
	}

	private AcquireTokenOnBehalfOfParameterBuilder WithUserAssertion(UserAssertion userAssertion)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithUserAssertion);
		Parameters.UserAssertion = userAssertion;
		return this;
	}

	public AcquireTokenOnBehalfOfParameterBuilder WithSendX5C(bool withSendX5C)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSendX5C, withSendX5C);
		Parameters.SendX5C = withSendX5C;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.ConfidentialClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenOnBehalfOf;
	}
}
