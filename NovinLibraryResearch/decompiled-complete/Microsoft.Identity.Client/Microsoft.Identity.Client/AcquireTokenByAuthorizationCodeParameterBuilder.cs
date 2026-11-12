using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenByAuthorizationCodeParameterBuilder : AbstractConfidentialClientAcquireTokenParameterBuilder<AcquireTokenByAuthorizationCodeParameterBuilder>
{
	private AcquireTokenByAuthorizationCodeParameters Parameters { get; } = new AcquireTokenByAuthorizationCodeParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenByAuthorizationCode;

	internal AcquireTokenByAuthorizationCodeParameterBuilder(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor)
		: base(confidentialClientApplicationExecutor)
	{
	}

	internal static AcquireTokenByAuthorizationCodeParameterBuilder Create(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor, IEnumerable<string> scopes, string authorizationCode)
	{
		return new AcquireTokenByAuthorizationCodeParameterBuilder(confidentialClientApplicationExecutor).WithScopes(scopes).WithAuthorizationCode(authorizationCode);
	}

	private AcquireTokenByAuthorizationCodeParameterBuilder WithAuthorizationCode(string authorizationCode)
	{
		Parameters.AuthorizationCode = authorizationCode;
		return this;
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenByAuthorizationCode;
	}

	protected override void Validate()
	{
		base.Validate();
		if (string.IsNullOrWhiteSpace(Parameters.AuthorizationCode))
		{
			throw new ArgumentException("AuthorizationCode can not be null or whitespace", "AuthorizationCode");
		}
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.ConfidentialClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	public AcquireTokenByAuthorizationCodeParameterBuilder WithSendX5C(bool withSendX5C)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSendX5C);
		Parameters.SendX5C = withSendX5C;
		return this;
	}
}
