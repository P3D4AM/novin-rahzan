using System.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRadarPointSeriesCoreXF : StiRadarSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "RadarPoint");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		StiRadarPointSeries stiRadarPointSeries = base.Series as StiRadarPointSeries;
		if (stiRadarPointSeries.AllowApplyStyle && !style.Core.MarkerVisible)
		{
			stiRadarPointSeries.Marker.Visible = true;
		}
	}

	public StiRadarPointSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
