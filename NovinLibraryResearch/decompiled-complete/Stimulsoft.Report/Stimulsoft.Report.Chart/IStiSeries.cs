using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiSeries : ICloneable, IStiJsonReportObject
{
	StiSeriesCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	StiTrendLinesCollection TrendLines { get; set; }

	IStiTrendLine TrendLine { get; set; }

	string Format { get; set; }

	string CoreTitle { get; set; }

	string TitleValue { get; set; }

	StiSeriesSortType SortBy { get; set; }

	StiSeriesSortDirection SortDirection { get; set; }

	bool ShowInLegend { get; set; }

	StiShowSeriesLabels ShowSeriesLabels { get; set; }

	bool ShowShadow { get; set; }

	StiChartFiltersCollection Filters { get; set; }

	IStiSeriesTopN TopN { get; set; }

	StiChartConditionsCollection Conditions { get; set; }

	StiSeriesYAxis YAxis { get; set; }

	IStiSeriesLabels SeriesLabels { get; set; }

	IStiChart Chart { get; set; }

	double?[] ValuesStart { get; set; }

	double?[] Values { get; set; }

	object[] Arguments { get; set; }

	object[] OriginalArguments { get; set; }

	string[] ToolTips { get; set; }

	object[] Tags { get; set; }

	string[] Hyperlinks { get; set; }

	IStiSeriesInteraction Interaction { get; set; }

	Color ProcessSeriesColors(int pointIndex, Color seriesColor);

	StiBrush ProcessSeriesBrushes(int pointIndex, StiBrush seriesBrush);

	StiMarkerType ProcessSeriesMarkerType(int pointIndex, StiMarkerType markerType);

	float ProcessSeriesMarkerAngle(int pointIndex, float markerAngle);

	bool ProcessSeriesMarkerVisible(int pointIndex);

	Type GetDefaultAreaType();
}
