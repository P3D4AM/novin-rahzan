using Microsoft.Identity.Client.AuthScheme.SSHCertificates;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.SSHCertificates;

public static class SSHExtensions
{
	public static AcquireTokenInteractiveParameterBuilder WithSSHCertificateAuthenticationScheme(this AcquireTokenInteractiveParameterBuilder builder, string publicKeyJwk, string keyId)
	{
		builder.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSSHAuthenticationScheme);
		builder.CommonParameters.AuthenticationScheme = new SSHCertAuthenticationScheme(keyId, publicKeyJwk);
		return builder;
	}

	public static AcquireTokenSilentParameterBuilder WithSSHCertificateAuthenticationScheme(this AcquireTokenSilentParameterBuilder builder, string publicKeyJwk, string keyId)
	{
		builder.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSSHAuthenticationScheme);
		builder.CommonParameters.AuthenticationScheme = new SSHCertAuthenticationScheme(keyId, publicKeyJwk);
		return builder;
	}
}
