using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRadarLineSeriesCoreXF : StiRadarSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "RadarLine");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiRadarLineSeries stiRadarLineSeries = base.Series as IStiRadarLineSeries;
		if (stiRadarLineSeries.AllowApplyStyle)
		{
			stiRadarLineSeries.LineColor = color;
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

	public StiRadarLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
