using System.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiOutsidePieLabelsCoreXF : StiCenterPieLabelsCoreXF
{
	public override int Position => 12;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsOutside");

	public override void ApplyStyle(IStiChartStyle style)
	{
		base.ApplyStyle(style);
		if (base.SeriesLabels.AllowApplyStyle)
		{
			((IStiOutsidePieLabels)base.SeriesLabels).LineColor = style.Core.SeriesLabelsLineColor;
		}
	}

	public Color GetLineColor(IStiSeries series, int colorIndex, int colorCount)
	{
		if (base.SeriesLabels.UseSeriesColor)
		{
			return GetSeriesLabelColor(series, colorIndex, colorCount);
		}
		return ((IStiOutsidePieLabels)base.SeriesLabels).LineColor;
	}

	public StiOutsidePieLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
