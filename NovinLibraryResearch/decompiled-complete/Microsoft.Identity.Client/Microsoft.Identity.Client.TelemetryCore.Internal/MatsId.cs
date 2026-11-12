using System;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class MatsId
{
	public static string Create()
	{
		return Guid.NewGuid().AsMatsCorrelationId();
	}

	public static string AsMatsCorrelationId(this Guid correlationIdGuid)
	{
		return correlationIdGuid.ToString("D");
	}
}
