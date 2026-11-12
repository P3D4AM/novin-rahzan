using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Data.Helpers;

public class StiUsedDataHelper
{
	public static List<string> GetMany(params IStiMeter[] meters)
	{
		if (meters == null)
		{
			return new List<string>();
		}
		return meters.Where((IStiMeter m) => m != null).SelectMany(GetSingle).Distinct()
			.ToList();
	}

	public static List<string> GetMany(IEnumerable<IStiMeter> meters)
	{
		if (meters == null)
		{
			return new List<string>();
		}
		return meters.Where((IStiMeter m) => m != null).SelectMany(GetSingle).Distinct()
			.ToList();
	}

	public static List<string> GetSingle(IStiMeter meter)
	{
		return GetSingle(meter.Expression);
	}

	public static List<string> GetSingle(string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
		{
			return new List<string>();
		}
		try
		{
			return StiExpressionHelper.GetArguments(expression);
		}
		catch
		{
		}
		return new List<string>();
	}
}
