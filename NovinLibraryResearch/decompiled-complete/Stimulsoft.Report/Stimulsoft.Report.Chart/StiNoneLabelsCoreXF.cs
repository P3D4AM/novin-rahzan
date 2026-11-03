using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiNoneLabelsCoreXF : StiSeriesLabelsCoreXF
{
	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.All;

	public override int Position => 0;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsNone");

	public StiNoneLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
