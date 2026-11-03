using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSteppedAreaSeriesCoreXF : StiSteppedLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SteppedArea");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiSteppedAreaSeries stiSteppedAreaSeries = base.Series as IStiSteppedAreaSeries;
		if (stiSteppedAreaSeries.AllowApplyStyle)
		{
			stiSteppedAreaSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	protected override void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		IStiSteppedLineSeries stiSteppedLineSeries = base.Series as IStiSteppedLineSeries;
		if (pointsInfo.Points != null && pointsInfo.Points.Length != 0 && (pointsInfo.Points.Length != 1 || stiSteppedLineSeries.PointAtCenter))
		{
			StiSteppedAreaSeriesGeom stiSteppedAreaSeriesGeom = new StiSteppedAreaSeriesGeom(geom, pointsInfo, base.Series);
			if (stiSteppedAreaSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiSteppedAreaSeriesGeom);
			}
		}
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		IStiSteppedAreaSeries stiSteppedAreaSeries = base.Series as IStiSteppedAreaSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return stiSteppedAreaSeries.Brush;
		}
		return seriesBrush;
	}

	public StiSteppedAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
