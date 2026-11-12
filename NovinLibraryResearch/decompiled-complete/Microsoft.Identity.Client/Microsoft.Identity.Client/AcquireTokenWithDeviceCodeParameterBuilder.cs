using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenWithDeviceCodeParameterBuilder : AbstractPublicClientAcquireTokenParameterBuilder<AcquireTokenWithDeviceCodeParameterBuilder>
{
	private AcquireTokenWithDeviceCodeParameters Parameters { get; } = new AcquireTokenWithDeviceCodeParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenWithDeviceCode;

	internal AcquireTokenWithDeviceCodeParameterBuilder(IPublicClientApplicationExecutor publicClientApplicationExecutor)
		: base(publicClientApplicationExecutor)
	{
	}

	internal static AcquireTokenWithDeviceCodeParameterBuilder Create(IPublicClientApplicationExecutor publicClientApplicationExecutor, IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback)
	{
		return new AcquireTokenWithDeviceCodeParameterBuilder(publicClientApplicationExecutor).WithScopes(scopes).WithDeviceCodeResultCallback(deviceCodeResultCallback);
	}

	public AcquireTokenWithDeviceCodeParameterBuilder WithDeviceCodeResultCallback(Func<DeviceCodeResult, Task> deviceCodeResultCallback)
	{
		Parameters.DeviceCodeResultCallback = deviceCodeResultCallback;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.PublicClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenByDeviceCode;
	}

	protected override void Validate()
	{
		base.Validate();
		if (Parameters.DeviceCodeResultCallback == null)
		{
			throw new ArgumentNullException("DeviceCodeResultCallback", "A deviceCodeResultCallback must be provided for Device Code authentication to work properly");
		}
	}
}
