using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenByIntegratedWindowsAuthParameters : AbstractAcquireTokenByUsernameParameters, IAcquireTokenParameters
{
	public void LogParameters(ICoreLogger logger)
	{
	}
}
