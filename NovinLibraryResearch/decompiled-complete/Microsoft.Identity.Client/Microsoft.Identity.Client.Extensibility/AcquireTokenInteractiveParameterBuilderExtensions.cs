using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.Extensibility;

public static class AcquireTokenInteractiveParameterBuilderExtensions
{
	public static AcquireTokenInteractiveParameterBuilder WithCustomWebUi(this AcquireTokenInteractiveParameterBuilder builder, ICustomWebUi customWebUi)
	{
		builder.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithCustomWebUi);
		builder.SetCustomWebUi(customWebUi);
		return builder;
	}
}
