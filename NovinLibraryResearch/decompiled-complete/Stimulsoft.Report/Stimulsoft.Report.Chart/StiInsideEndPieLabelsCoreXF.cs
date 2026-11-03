using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiInsideEndPieLabelsCoreXF : StiCenterPieLabelsCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsInsideEnd");

	public override int Position => 10;

	public StiInsideEndPieLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
