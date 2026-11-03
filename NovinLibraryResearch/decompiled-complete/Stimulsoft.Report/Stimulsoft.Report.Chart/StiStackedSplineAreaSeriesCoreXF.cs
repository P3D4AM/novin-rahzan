using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineAreaSeriesCoreXF : StiStackedSplineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedSplineArea");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiStackedSplineAreaSeries stiStackedSplineAreaSeries = base.Series as IStiStackedSplineAreaSeries;
		if (stiStackedSplineAreaSeries.AllowApplyStyle)
		{
			stiStackedSplineAreaSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	protected override void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.PointsStart != null && pointsInfo.PointsEnd != null)
		{
			StiStackedSplineAreaSeriesGeom stiStackedSplineAreaSeriesGeom = new StiStackedSplineAreaSeriesGeom(geom, pointsInfo, base.Series);
			if (stiStackedSplineAreaSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiStackedSplineAreaSeriesGeom);
			}
		}
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		IStiStackedSplineAreaSeries stiStackedSplineAreaSeries = base.Series as IStiStackedSplineAreaSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return stiStackedSplineAreaSeries.Brush;
		}
		return seriesBrush;
	}

	public StiStackedSplineAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
