using System.Drawing;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Base.Context;

public class StiSeriesInteractionData : StiInteractionData
{
	public bool IsElements { get; set; } = true;


	public object Tag { get; private set; }

	public string Tooltip { get; private set; }

	public string Hyperlink { get; private set; }

	public object Argument { get; private set; }

	public object OriginalArgument { get; private set; }

	public double? EndValue { get; private set; }

	public IStiSeries Series { get; set; }

	public int PointIndex { get; private set; }

	public PointF? Point { get; set; }

	public void Fill(IStiArea area, IStiSeries series, int pointIndex)
	{
		if (area is IStiAxisArea && ((IStiAxisArea)area).ReverseHor)
		{
			PointIndex = series.Values.Length - pointIndex - 1;
			Series = series;
			if (pointIndex >= 0 && pointIndex < series.Arguments.Length)
			{
				Argument = series.Arguments[series.Arguments.Length - pointIndex - 1];
			}
			if (series.OriginalArguments != null && pointIndex >= 0 && pointIndex < series.OriginalArguments.Length)
			{
				OriginalArgument = series.OriginalArguments[series.OriginalArguments.Length - pointIndex - 1];
			}
			if (pointIndex >= 0 && pointIndex < series.Values.Length)
			{
				base.Value = series.Values[series.Values.Length - pointIndex - 1];
			}
			if (pointIndex >= 0 && pointIndex < series.Tags.Length)
			{
				Tag = series.Tags[series.Tags.Length - pointIndex - 1];
			}
			if (pointIndex >= 0 && pointIndex < series.Hyperlinks.Length)
			{
				Hyperlink = series.Hyperlinks[series.Hyperlinks.Length - pointIndex - 1];
			}
			if (pointIndex >= 0 && pointIndex < series.ToolTips.Length)
			{
				Tooltip = series.ToolTips[series.ToolTips.Length - pointIndex - 1];
			}
			if (series is IStiRangeSeries stiRangeSeries && pointIndex >= 0 && pointIndex < stiRangeSeries.ValuesEnd.Length)
			{
				EndValue = stiRangeSeries.ValuesEnd[stiRangeSeries.ValuesEnd.Length - pointIndex - 1];
			}
		}
		else
		{
			PointIndex = pointIndex;
			Series = series;
			if (pointIndex >= 0 && pointIndex < series.Arguments.Length)
			{
				Argument = series.Arguments[pointIndex];
			}
			if (series.OriginalArguments != null && pointIndex >= 0 && pointIndex < series.OriginalArguments.Length)
			{
				OriginalArgument = series.OriginalArguments[pointIndex];
			}
			if (pointIndex >= 0 && pointIndex < series.Values.Length)
			{
				base.Value = series.Values[pointIndex];
			}
			if (pointIndex >= 0 && pointIndex < series.Tags.Length)
			{
				Tag = series.Tags[pointIndex];
			}
			if (pointIndex >= 0 && pointIndex < series.Hyperlinks.Length)
			{
				Hyperlink = series.Hyperlinks[pointIndex];
			}
			if (pointIndex >= 0 && pointIndex < series.ToolTips.Length)
			{
				Tooltip = series.ToolTips[pointIndex];
			}
			if (series is IStiRangeSeries stiRangeSeries2 && pointIndex >= 0 && pointIndex < stiRangeSeries2.ValuesEnd.Length)
			{
				EndValue = stiRangeSeries2.ValuesEnd[pointIndex];
			}
		}
	}

	public StiSeriesInteractionData()
		: this(null, null, 0)
	{
	}

	public StiSeriesInteractionData(IStiArea area, IStiSeries series, int pointIndex)
	{
		if (area != null || series != null)
		{
			Fill(area, series, pointIndex);
		}
	}
}
