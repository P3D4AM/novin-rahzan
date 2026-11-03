using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiAreaSeriesCoreXF : StiLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Area");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiAreaSeries stiAreaSeries = base.Series as IStiAreaSeries;
		if (stiAreaSeries.AllowApplyStyle)
		{
			stiAreaSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	protected override void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null && pointsInfo.Points.Length > 1)
		{
			StiAreaSeriesGeom stiAreaSeriesGeom = new StiAreaSeriesGeom(geom, pointsInfo, base.Series);
			if (stiAreaSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiAreaSeriesGeom);
			}
		}
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		IStiAreaSeries stiAreaSeries = base.Series as IStiAreaSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return stiAreaSeries.Brush;
		}
		return seriesBrush;
	}

	public StiAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
