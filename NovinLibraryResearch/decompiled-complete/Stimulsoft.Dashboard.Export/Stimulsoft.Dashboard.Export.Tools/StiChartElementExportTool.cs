using System.Collections.Generic;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiChartElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		Render(element, destination, rect, settings, null);
	}

	internal void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings, List<double?[]> chartStartValues)
	{
		if (RenderEmptyDataMessage(element, destination, rect, settings))
		{
			return;
		}
		StiChartElement stiChartElement = element as StiChartElement;
		StiDataTable dataTable = ((stiChartElement.ChartValuesList().Count > 0) ? StiElementExportTool.GetDataTable(element) : null);
		StiChart stiChart = StiChartElementBuilder.Create(stiChartElement).Render(stiChartElement, dataTable);
		for (int i = 0; i < stiChart.Series.Count; i++)
		{
			if (chartStartValues == null)
			{
				break;
			}
			if (i >= chartStartValues.Count)
			{
				break;
			}
			stiChart.Series[i].ValuesStart = chartStartValues[i];
		}
		stiChart.Name = element.Name + "_Content";
		stiChart.ClientRectangle = rect;
		destination.Components.Add(stiChart);
	}
}
