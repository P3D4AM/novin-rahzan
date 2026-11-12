using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal interface IAcquireTokenParameters
{
	void LogParameters(ICoreLogger logger);
}
