using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Functions;

namespace Stimulsoft.Data.Helpers;

public static class StiLabelHelper
{
	private static Dictionary<string, string> cache = new Dictionary<string, string>();

	public static string GetLabel(IStiMeter meter)
	{
		if (meter == null)
		{
			return "Unknown";
		}
		if (!string.IsNullOrEmpty(meter.Label))
		{
			return meter.Label;
		}
		string text = (meter as IStiLocalizedMeter)?.LocalizedName;
		try
		{
			lock (cache)
			{
				if (!string.IsNullOrWhiteSpace(meter.Expression) && cache.ContainsKey(meter.Expression))
				{
					return cache[meter.Expression];
				}
				string text2 = StiExpressionHelper.GetFirstArgumentFromExpression(meter.Expression);
				if (text2 == null)
				{
					text2 = StiExpressionHelper.GetFunction(meter.Expression);
				}
				if (text2 == null && text != null)
				{
					text2 = text;
				}
				if (text2.Contains("."))
				{
					text2 = text2.Split(new char[1] { '.' }).LastOrDefault();
				}
				if (text2.Length <= 0 || !char.IsUpper(text2[0]))
				{
					text2 = Funcs.ToProperCase(text2);
				}
				if (!string.IsNullOrWhiteSpace(meter.Expression))
				{
					cache.Add(meter.Expression, text2);
				}
				return text2;
			}
		}
		catch
		{
			if (!string.IsNullOrWhiteSpace(meter.Expression))
			{
				cache.Add(meter.Expression, text);
			}
			return text;
		}
	}
}
