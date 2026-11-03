using System.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiSeriesPointsInfo
{
	public PointF?[] Points { get; set; }

	public PointF?[] PointsFrom { get; set; }

	public PointF?[] PointsStart { get; set; }

	public PointF?[] PointsEnd { get; set; }

	public PointF?[] PointsZeroConnect { get; set; }

	public PointF?[] PointsNullConnect { get; set; }

	public string[] PointsIds { get; set; }

	public string AdditionalSeriesId { get; set; } = "";

}
