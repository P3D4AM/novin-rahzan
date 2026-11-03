using System.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiCustomProgressElementStyle : StiProgressElementStyle
{
	private string name;

	public override StiComponentId ComponentId => StiComponentId.StiCustomDashboardProgressStyle;

	public override string LocalizedName => name;

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Custom;

	public override Color TrackColor { get; set; }

	public override Color BandColor { get; set; }

	public override Color[] SeriesColors { get; set; }

	public override Color BackColor { get; set; }

	public StiCustomProgressElementStyle(StiProgressStyle style)
	{
		name = style.Name;
		TrackColor = style.TrackColor;
		BandColor = style.BandColor;
		SeriesColors = style.SeriesColors;
		ForeColor = style.ForeColor;
		BackColor = style.BackColor;
	}
}
