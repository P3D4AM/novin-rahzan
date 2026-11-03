using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSplineAreaSeriesCoreXF : StiSplineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SplineArea");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiSplineAreaSeries stiSplineAreaSeries = base.Series as IStiSplineAreaSeries;
		if (stiSplineAreaSeries.AllowApplyStyle)
		{
			stiSplineAreaSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	protected override void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null)
		{
			StiSplineAreaSeriesGeom stiSplineAreaSeriesGeom = new StiSplineAreaSeriesGeom(geom, pointsInfo, base.Series);
			if (stiSplineAreaSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiSplineAreaSeriesGeom);
			}
		}
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		IStiSplineAreaSeries stiSplineAreaSeries = base.Series as IStiSplineAreaSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return stiSplineAreaSeries.Brush;
		}
		return seriesBrush;
	}

	public StiSplineAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
