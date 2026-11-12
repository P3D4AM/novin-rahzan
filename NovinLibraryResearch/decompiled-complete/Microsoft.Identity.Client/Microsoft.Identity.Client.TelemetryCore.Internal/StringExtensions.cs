using System;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class StringExtensions
{
	public static string TrimCurlyBraces(this string input)
	{
		return Guid.Parse(input).ToString("D");
	}
}
