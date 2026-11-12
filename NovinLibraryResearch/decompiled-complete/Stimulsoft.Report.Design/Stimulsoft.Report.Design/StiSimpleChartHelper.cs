using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design;

internal static class StiSimpleChartHelper
{
	public static StiChart GetSimpleChart(StiChart chart)
	{
		StiChart stiChart = (StiChart)chart.Clone();
		if (stiChart.Area is StiAxisArea stiAxisArea)
		{
			stiAxisArea.AxisCore.SwitchOff();
			stiAxisArea.XAxis.StartFromZero = true;
			stiAxisArea.GridLinesHorRight.Visible = false;
			stiAxisArea.GridLinesHor.MinorVisible = false;
			stiAxisArea.GridLinesVert.MinorVisible = false;
			stiAxisArea.GridLinesHorRight.MinorVisible = false;
			stiAxisArea.GridLinesHor.AllowApplyStyle = true;
			stiAxisArea.GridLinesVert.AllowApplyStyle = true;
			stiAxisArea.GridLinesHorRight.AllowApplyStyle = true;
			stiAxisArea.InterlacingHor.AllowApplyStyle = true;
			stiAxisArea.InterlacingVert.AllowApplyStyle = true;
		}
		AddDefaultSeries(stiChart);
		stiChart.ConstantLines.Clear();
		stiChart.Strips.Clear();
		stiChart.Core.ApplyStyle(stiChart.Style);
		stiChart.Brush = new StiEmptyBrush();
		stiChart.Area.Brush = new StiEmptyBrush();
		stiChart.Legend.Visible = false;
		stiChart.SeriesLabels.Visible = false;
		stiChart.Title.Visible = false;
		stiChart.Table.Visible = false;
		stiChart.AllowApplyStyle = true;
		stiChart.Area.AllowApplyStyle = true;
		stiChart.Series.OfType<StiPie3dSeries>().ToList().ForEach(delegate(StiPie3dSeries s)
		{
			s.Options3D.Height = 10f;
		});
		stiChart.Series.OfType<StiPie3dSeries>().ToList().ForEach(delegate(StiPie3dSeries s)
		{
			s.Options3D.Distance = 2f;
		});
		return stiChart;
	}

	public static StiChart GetSimpleChart(IStiChartElement element)
	{
		StiChart stiChart = new StiChart();
		stiChart.Page = element.Page;
		stiChart.Series.Clear();
		stiChart.Series.Add(element.GetChartSeries());
		if (stiChart.Area is StiAxisArea stiAxisArea)
		{
			stiAxisArea.AxisCore.SwitchOff();
		}
		AddDefaultSeries(stiChart);
		stiChart.ConstantLines.Clear();
		stiChart.Strips.Clear();
		stiChart.Core.ApplyStyle(stiChart.Style);
		stiChart.Brush = new StiEmptyBrush();
		stiChart.Area.Brush = new StiEmptyBrush();
		stiChart.Legend.Visible = false;
		stiChart.SeriesLabels.Visible = false;
		stiChart.Title.Visible = false;
		stiChart.Table.Visible = false;
		stiChart.AllowApplyStyle = true;
		stiChart.Area.AllowApplyStyle = true;
		stiChart.Series.OfType<StiPie3dSeries>().ToList().ForEach(delegate(StiPie3dSeries s)
		{
			s.Options3D.Height = 10f;
		});
		stiChart.Series.OfType<StiPie3dSeries>().ToList().ForEach(delegate(StiPie3dSeries s)
		{
			s.Options3D.Distance = 2f;
		});
		return stiChart;
	}

	private static void AddDefaultSeries(StiChart chart)
	{
		Type type = ((chart.Series.Count > 0) ? chart.Series[0].GetType() : chart.Area.GetDefaultSeriesType());
		StiSeries stiSeries = (StiSeries)StiActivator.CreateObject(type);
		Type type2 = ((chart.Series.Count > 1) ? chart.Series[1].GetType() : type);
		StiSeries stiSeries2 = (StiSeries)StiActivator.CreateObject(type2);
		chart.Series.Clear();
		if (stiSeries is IStiSunburstSeries || stiSeries2 is IStiSunburstSeries)
		{
			object[] arguments = new string[4] { "A", "A", "B", "C" };
			stiSeries.Arguments = arguments;
			arguments = new string[4] { "C", "D", "E", "" };
			stiSeries2.Arguments = arguments;
		}
		if (stiSeries2 is StiParetoSeries)
		{
			stiSeries.Values = new double?[3] { 4.0, 2.0, 1.0 };
		}
		chart.Series.Add(stiSeries);
		if (!(stiSeries2 is StiFunnelSeries) && !(stiSeries2 is StiParetoSeries))
		{
			chart.Series.Add(stiSeries2);
		}
		chart.Series.ApplyStyle(chart.Style);
	}
}
