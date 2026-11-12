using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Data.Extensions;

public static class StiMeterExt
{
	public static int IndexOf(this IEnumerable<IStiMeter> meters, IStiMeter meter)
	{
		if (meter == null)
		{
			return -1;
		}
		return meters.ToList().IndexOf(meter);
	}

	public static IEnumerable<IStiDimensionMeter> GetDimensions(this IEnumerable<IStiMeter> meters)
	{
		return meters?.Where((IStiMeter m) => m is IStiDimensionMeter).Cast<IStiDimensionMeter>();
	}
}
