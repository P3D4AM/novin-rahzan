using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Visuals.Indicator;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Render;

internal class StiIndicatorElementBuilder : StiElementBuilder
{
	public IEnumerable<StiIndicatorIteration> Render(StiIndicatorElement element, StiDataTable dataTable)
	{
		CultureInfo culture = StiCultureHelper.Set(element?.Report);
		try
		{
			int valueIndex = GetValueMeterIndex(dataTable);
			int targetIndex = GetTargetMeterIndex(dataTable);
			int seriesIndex = GetSeriesMeterIndex(dataTable);
			IEnumerable<string> seriesKeys = GetSeriesKeys(dataTable, seriesIndex, GetSeriesKeysLimitCount(element));
			int seriesKeysCount = seriesKeys.Count();
			IEnumerable<StiIndicatorIteration> iterations = seriesKeys.Select((string seriesKey) => new StiIndicatorIteration
			{
				Series = GetSeries(dataTable, seriesIndex, seriesKey, element.Report),
				Value = GetValue(dataTable, valueIndex, seriesIndex, seriesKeysCount, seriesKey),
				Target = GetNullableValue(dataTable, targetIndex, seriesIndex, seriesKeysCount, seriesKey)
			});
			iterations = ProcessVariationSort(element, iterations);
			return ProcessTopNElements(element, iterations);
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	private static IEnumerable<StiIndicatorIteration> ProcessVariationSort(StiIndicatorElement element, IEnumerable<StiIndicatorIteration> iterations)
	{
		List<StiDataSortRule> userSorts = element.UserSorts;
		if (userSorts != null && userSorts.Any((StiDataSortRule r) => r.Key == StiDataSortVariation.Ident))
		{
			iterations = ((userSorts.FirstOrDefault().Direction != StiDataSortDirection.Ascending) ? iterations.OrderByDescending(delegate(StiIndicatorIteration i)
			{
				decimal? target2 = i.Target;
				if ((target2.GetValueOrDefault() == default(decimal)) & target2.HasValue)
				{
					return default(decimal);
				}
				decimal value2 = i.Value;
				target2 = i.Target;
				return (decimal?)value2 / target2;
			}) : iterations.OrderBy(delegate(StiIndicatorIteration i)
			{
				decimal? target = i.Target;
				if ((target.GetValueOrDefault() == default(decimal)) & target.HasValue)
				{
					return default(decimal);
				}
				decimal value = i.Value;
				target = i.Target;
				return (decimal?)value / target;
			}));
		}
		return iterations;
	}

	internal static IEnumerable<StiIndicatorIteration> ProcessTopNElements(StiIndicatorElement element, IEnumerable<StiIndicatorIteration> iterations)
	{
		if (element.TopN.Mode == StiDataTopNMode.None)
		{
			return iterations;
		}
		iterations = ((element.TopN.Mode == StiDataTopNMode.Bottom) ? iterations.OrderBy((StiIndicatorIteration e) => e.Value).ToList() : iterations.OrderByDescending((StiIndicatorIteration e) => e.Value).ToList());
		List<StiIndicatorIteration> list = iterations.Take(element.TopN.Count).ToList();
		if (iterations.Count() > element.TopN.Count && element.TopN.ShowOthers)
		{
			decimal value = iterations.Skip(element.TopN.Count).Sum((StiIndicatorIteration c) => StiValueHelper.TryToDecimal(c.Value));
			IEnumerable<StiIndicatorIteration> source = iterations.Skip(element.TopN.Count);
			decimal? target = (source.All((StiIndicatorIteration c) => c == null || !c.Target.HasValue) ? null : source.Sum((StiIndicatorIteration c) => StiValueHelper.TryToNullableDecimal(c.Target)));
			string series = (string.IsNullOrEmpty(element.TopN.OthersText) ? Loc.Get("FormDesigner", "Others") : StiReportParser.Parse(element.TopN.OthersText, element));
			list.Add(new StiIndicatorIteration
			{
				Series = series,
				Value = value,
				Target = target
			});
		}
		return list;
	}

	private int GetValueMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiValueIndicatorMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private int GetTargetMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiTargetIndicatorMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private int GetSeriesMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiSeriesIndicatorMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}
}
