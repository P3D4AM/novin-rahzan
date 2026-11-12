using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Visuals.Progress;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Render;

internal class StiProgressElementBuilder : StiElementBuilder
{
	public IEnumerable<StiProgressIteration> Render(StiProgressElement element, StiDataTable dataTable)
	{
		CultureInfo culture = StiCultureHelper.Set(element?.Report);
		try
		{
			int valueIndex = GetValueMeterIndex(dataTable);
			int targetIndex = GetTargetMeterIndex(dataTable);
			int seriesIndex = GetSeriesMeterIndex(dataTable);
			IEnumerable<string> seriesKeys = GetSeriesKeys(dataTable, seriesIndex, GetSeriesKeysLimitCount(element));
			int seriesKeysCount = seriesKeys.Count();
			IEnumerable<StiProgressIteration> iterations = seriesKeys.Select((string seriesKey) => new StiProgressIteration
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

	private static IEnumerable<StiProgressIteration> ProcessVariationSort(StiProgressElement element, IEnumerable<StiProgressIteration> iterations)
	{
		List<StiDataSortRule> userSorts = element.UserSorts;
		if (userSorts != null && userSorts.Any((StiDataSortRule r) => r.Key == StiDataSortVariation.Ident))
		{
			iterations = ((userSorts.FirstOrDefault().Direction != StiDataSortDirection.Ascending) ? iterations.OrderByDescending(delegate(StiProgressIteration i)
			{
				decimal? target2 = i.Target;
				if ((target2.GetValueOrDefault() == default(decimal)) & target2.HasValue)
				{
					return default(decimal);
				}
				decimal value2 = i.Value;
				target2 = i.Target;
				return (decimal?)value2 / target2;
			}) : iterations.OrderBy(delegate(StiProgressIteration i)
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

	internal static IEnumerable<StiProgressIteration> ProcessTopNElements(StiProgressElement element, IEnumerable<StiProgressIteration> iterations)
	{
		if (element.TopN.Mode == StiDataTopNMode.None)
		{
			return iterations;
		}
		iterations = ((element.TopN.Mode == StiDataTopNMode.Bottom) ? iterations.OrderBy((StiProgressIteration e) => e.Value).ToList() : iterations.OrderByDescending((StiProgressIteration e) => e.Value).ToList());
		List<StiProgressIteration> list = iterations.Take(element.TopN.Count).ToList();
		if (iterations.Count() > element.TopN.Count && element.TopN.ShowOthers)
		{
			decimal value = iterations.Skip(element.TopN.Count).Sum((StiProgressIteration c) => StiValueHelper.TryToDecimal(c.Value));
			IEnumerable<StiProgressIteration> source = iterations.Skip(element.TopN.Count);
			decimal? target = (source.All((StiProgressIteration c) => c == null || !c.Target.HasValue) ? null : source.Sum((StiProgressIteration c) => StiValueHelper.TryToNullableDecimal(c.Target)));
			string series = (string.IsNullOrEmpty(element.TopN.OthersText) ? Loc.Get("FormDesigner", "Others") : StiReportParser.Parse(element.TopN.OthersText, element));
			list.Add(new StiProgressIteration
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
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiValueProgressMeter);
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
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiTargetProgressMeter);
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
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiSeriesProgressMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}
}
