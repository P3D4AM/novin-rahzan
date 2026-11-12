using System.Text;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenForClientParameters : IAcquireTokenParameters
{
	public bool ForceRefresh { get; set; }

	public bool SendX5C { get; set; }

	public bool AutoDetectRegion { get; set; }

	public void LogParameters(ICoreLogger logger)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("=== AcquireTokenForClientParameters ===");
		stringBuilder.AppendLine("SendX5C: " + SendX5C);
		stringBuilder.AppendLine("WithAzureRegion: " + AutoDetectRegion);
		stringBuilder.AppendLine("ForceRefresh: " + ForceRefresh);
		logger.Info(stringBuilder.ToString());
	}
}
