using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenByAuthorizationCodeParameters : IAcquireTokenParameters
{
	public string AuthorizationCode { get; set; }

	public bool SendX5C { get; set; }

	public void LogParameters(ICoreLogger logger)
	{
	}
}
