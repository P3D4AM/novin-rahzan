using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Gauge.Helpers;

public static class StiTickLabelHelper
{
	private class CacheInfo
	{
		public float ValueKey { get; }

		public string ValueStr { get; }

		public int Count { get; set; }

		public override string ToString()
		{
			return $"{ValueStr} - {Count}";
		}

		public CacheInfo(float valueKey, string valueStr, int count)
		{
			ValueKey = valueKey;
			ValueStr = valueStr;
			Count = count;
		}
	}

	public static Dictionary<float, string> GetLabels(Dictionary<float, float> collection)
	{
		Dictionary<float, string> result = new Dictionary<float, string>();
		List<CacheInfo> list = new List<CacheInfo>();
		foreach (KeyValuePair<float, float> item in collection)
		{
			double num = ((item.Key < 5f) ? Math.Round(item.Key, 2) : Math.Round(item.Key));
			if (num == 0.0)
			{
				result.Add(item.Key, "0");
				continue;
			}
			bool flag = false;
			if (num < 0.0)
			{
				flag = true;
				num = Math.Abs(num);
			}
			string valueStr;
			if (num < 1000.0)
			{
				valueStr = num.ToString(CultureInfo.InvariantCulture);
			}
			else if (num < 1000000.0)
			{
				valueStr = $"{(int)(num / 1000.0)}K";
			}
			else if (num < 1000000000.0)
			{
				valueStr = $"{(int)(num / 1000000.0)}M";
			}
			else if (num < 1000000000000.0)
			{
				valueStr = $"{(int)(num / 1000000000.0)}B";
			}
			else if (num < 1000000000000000.0)
			{
				valueStr = $"{(int)(num / 1000000000000.0)}T";
			}
			else
			{
				valueStr = num.ToString(CultureInfo.InvariantCulture);
			}
			if (flag)
			{
				valueStr = "-" + valueStr;
			}
			CacheInfo cacheInfo = list.FirstOrDefault((CacheInfo x) => x.ValueStr == valueStr);
			if (cacheInfo == null)
			{
				list.Add(new CacheInfo(item.Key, valueStr, 1));
			}
			else
			{
				cacheInfo.Count++;
			}
			result.Add(item.Key, valueStr);
		}
		foreach (CacheInfo item2 in list)
		{
			if (item2.Count > 1)
			{
				Prepare(item2, ref result);
			}
		}
		return result;
	}

	private static void Prepare(CacheInfo cache, ref Dictionary<float, string> result)
	{
		float[] array = result.Keys.ToArray();
		float[] array2 = array;
		foreach (float num in array2)
		{
			string text = result[num];
			if (!(text != cache.ValueStr))
			{
				result[num] = StiAbbreviationNumberFormatHelper.Format(num);
			}
		}
	}
}
