using System;

namespace Microsoft.IdentityModel.Tokens;

public static class EpochTime
{
	public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	public static long GetIntDate(DateTime datetime)
	{
		DateTime dateTime = datetime;
		if (datetime.Kind != DateTimeKind.Utc)
		{
			dateTime = datetime.ToUniversalTime();
		}
		if (dateTime.ToUniversalTime() <= UnixEpoch)
		{
			return 0L;
		}
		return (long)(dateTime - UnixEpoch).TotalSeconds;
	}

	public static DateTime DateTime(long secondsSinceUnixEpoch)
	{
		if (secondsSinceUnixEpoch <= 0)
		{
			return UnixEpoch;
		}
		double num = secondsSinceUnixEpoch;
		TimeSpan maxValue = TimeSpan.MaxValue;
		if (num > maxValue.TotalSeconds)
		{
			return DateTimeUtil.Add(UnixEpoch, TimeSpan.MaxValue).ToUniversalTime();
		}
		return DateTimeUtil.Add(UnixEpoch, TimeSpan.FromSeconds(secondsSinceUnixEpoch)).ToUniversalTime();
	}
}
