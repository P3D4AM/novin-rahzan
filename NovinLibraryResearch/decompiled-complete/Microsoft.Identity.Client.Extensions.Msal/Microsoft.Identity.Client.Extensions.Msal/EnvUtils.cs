using System;
using System.Diagnostics;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal static class EnvUtils
{
	internal const string TraceLevelEnvVarName = "IDENTITYEXTENSIONTRACELEVEL";

	internal static TraceSource GetNewTraceSource(string sourceName)
	{
		SourceLevels defaultLevel = SourceLevels.Warning;
		string environmentVariable = Environment.GetEnvironmentVariable("IDENTITYEXTENSIONTRACELEVEL");
		if (!string.IsNullOrEmpty(environmentVariable) && Enum.TryParse<SourceLevels>(environmentVariable, ignoreCase: true, out var result))
		{
			defaultLevel = result;
		}
		return new TraceSource("Microsoft.Identity.Client.Extensions.TraceSource", defaultLevel);
	}
}
