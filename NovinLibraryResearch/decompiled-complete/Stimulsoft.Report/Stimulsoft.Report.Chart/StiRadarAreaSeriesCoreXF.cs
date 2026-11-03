using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRadarAreaSeriesCoreXF : StiRadarSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "RadarArea");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiRadarAreaSeries stiRadarAreaSeries = base.Series as IStiRadarAreaSeries;
		if (stiRadarAreaSeries.AllowApplyStyle)
		{
			stiRadarAreaSeries.Brush = style.Core.GetAreaBrush(color);
			stiRadarAreaSeries.LineColor = color;
		}
	}

	public override void RenderLines(StiContext context, IStiRadarSeries series, StiSeriesPointsInfo pointsInfo, StiAreaGeom geom)
	{
		StiLineSeriesGeom item = new StiLineSeriesGeom(geom, pointsInfo, series);
		if (geom != null)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	public override void RenderAreas(StiContext context, IStiRadarSeries series, StiSeriesPointsInfo pointsInfo, StiAreaGeom geom)
	{
		StiRadarAreaSeriesGeom item = new StiRadarAreaSeriesGeom(series, pointsInfo);
		if (geom != null)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	public StiRadarAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
