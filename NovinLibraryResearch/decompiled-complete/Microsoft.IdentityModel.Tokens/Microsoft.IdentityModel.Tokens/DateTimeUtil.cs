using System;

namespace Microsoft.IdentityModel.Tokens;

public static class DateTimeUtil
{
	public static DateTime Add(DateTime time, TimeSpan timespan)
	{
		if (timespan == TimeSpan.Zero)
		{
			return time;
		}
		if (timespan > TimeSpan.Zero && DateTime.MaxValue - time <= timespan)
		{
			return GetMaxValue(time.Kind);
		}
		if (timespan < TimeSpan.Zero && DateTime.MinValue - time >= timespan)
		{
			return GetMinValue(time.Kind);
		}
		return time + timespan;
	}

	public static DateTime GetMaxValue(DateTimeKind kind)
	{
		DateTime maxValue;
		if (kind == DateTimeKind.Unspecified)
		{
			maxValue = DateTime.MaxValue;
			return new DateTime(maxValue.Ticks, DateTimeKind.Utc);
		}
		maxValue = DateTime.MaxValue;
		return new DateTime(maxValue.Ticks, kind);
	}

	public static DateTime GetMinValue(DateTimeKind kind)
	{
		DateTime minValue;
		if (kind == DateTimeKind.Unspecified)
		{
			minValue = DateTime.MinValue;
			return new DateTime(minValue.Ticks, DateTimeKind.Utc);
		}
		minValue = DateTime.MinValue;
		return new DateTime(minValue.Ticks, kind);
	}

	public static DateTime? ToUniversalTime(DateTime? value)
	{
		if (!value.HasValue || value.Value.Kind == DateTimeKind.Utc)
		{
			return value;
		}
		return ToUniversalTime(value.Value);
	}

	public static DateTime ToUniversalTime(DateTime value)
	{
		if (value.Kind == DateTimeKind.Utc)
		{
			return value;
		}
		return value.ToUniversalTime();
	}
}
