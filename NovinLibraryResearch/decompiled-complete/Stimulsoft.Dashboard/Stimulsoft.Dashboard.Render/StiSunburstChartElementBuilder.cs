using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Render;

internal class StiSunburstChartElementBuilder : StiChartElementBuilder
{
	protected override void RenderElements(StiChartElement element, StiChart chart, StiDataTable dataTable)
	{
		IEnumerable<StiMeter> argumentMeters = GetArgumentMeters(dataTable);
		StiMeter value = GetValueMeters(dataTable).FirstOrDefault();
		int num = GetValueMeterIndexes(dataTable).FirstOrDefault();
		for (int i = 0; i < argumentMeters.Count(); i++)
		{
			int num2 = GetArgumentMeterIndexes(dataTable)[i];
			StiSeries stiSeries = RenderSeries(element, value, "", chart);
			if (stiSeries != null)
			{
				stiSeries.Values = new double?[dataTable.Rows.Count];
				stiSeries.Arguments = new object[dataTable.Rows.Count];
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					stiSeries.Values[j] = StiValueHelper.TryToNullableDouble(dataTable.Rows[j][num]);
					stiSeries.Arguments[j] = dataTable.Rows[j][num2];
				}
			}
		}
	}

	protected override StiSeries RenderSeries(StiChartElement element, StiMeter value, string seriesKey, StiChart chart)
	{
		StiSeries stiSeries = StiChartSeriesCreator.New(((StiValueChartMeter)value).SeriesType);
		stiSeries.Core.SeriesColors = ((element.SeriesColors != null && element.SeriesColors.Length != 0) ? element.SeriesColors : null);
		stiSeries.IsDashboard = true;
		chart.Series.Add(stiSeries);
		return stiSeries;
	}
}
