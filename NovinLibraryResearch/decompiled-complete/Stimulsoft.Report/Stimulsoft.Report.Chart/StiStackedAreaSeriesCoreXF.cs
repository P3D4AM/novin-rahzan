using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedAreaSeriesCoreXF : StiStackedLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedArea");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiStackedAreaSeries stiStackedAreaSeries = base.Series as IStiStackedAreaSeries;
		if (stiStackedAreaSeries.AllowApplyStyle)
		{
			stiStackedAreaSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	protected override void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.PointsStart != null && pointsInfo.PointsStart.Length > 1 && pointsInfo.PointsEnd != null && pointsInfo.PointsEnd.Length > 1)
		{
			StiStackedAreaSeriesGeom stiStackedAreaSeriesGeom = new StiStackedAreaSeriesGeom(geom, pointsInfo, base.Series);
			if (stiStackedAreaSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiStackedAreaSeriesGeom);
			}
		}
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		IStiStackedAreaSeries stiStackedAreaSeries = base.Series as IStiStackedAreaSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return stiStackedAreaSeries.Brush;
		}
		return seriesBrush;
	}

	public StiStackedAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
