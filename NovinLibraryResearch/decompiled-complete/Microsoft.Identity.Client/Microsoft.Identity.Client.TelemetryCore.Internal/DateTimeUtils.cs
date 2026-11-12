using System;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class DateTimeUtils
{
	public static long GetMillisecondsSinceEpoch(DateTime timeUtc)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return Convert.ToInt64((timeUtc - dateTime).TotalMilliseconds);
	}
}
