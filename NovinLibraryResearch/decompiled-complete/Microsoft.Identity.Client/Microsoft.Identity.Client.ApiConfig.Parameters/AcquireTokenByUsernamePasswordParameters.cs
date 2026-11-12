using System.Security;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenByUsernamePasswordParameters : AbstractAcquireTokenByUsernameParameters, IAcquireTokenParameters
{
	public SecureString Password { get; set; }

	public void LogParameters(ICoreLogger logger)
	{
	}
}
