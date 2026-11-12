using System.Text;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenOnBehalfOfParameters : IAcquireTokenParameters
{
	public UserAssertion UserAssertion { get; set; }

	public bool SendX5C { get; set; }

	public void LogParameters(ICoreLogger logger)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("=== OnBehalfOfParameters ===");
		stringBuilder.AppendLine("SendX5C: " + SendX5C);
		logger.Info(stringBuilder.ToString());
	}
}
