using System.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiCustomIndicatorElementStyle : StiIndicatorElementStyle
{
	private string name;

	public override StiComponentId ComponentId => StiComponentId.StiCustomDashboardIndicatorStyle;

	public override string LocalizedName => name;

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Custom;

	public override Color GlyphColor { get; set; }

	public override Color ForeColor { get; set; }

	public StiCustomIndicatorElementStyle(StiIndicatorStyle style)
	{
		name = style.Name;
		GlyphColor = style.GlyphColor;
		ForeColor = style.ForeColor;
		BackColor = style.BackColor;
		HotBackColor = style.HotBackColor;
		PositiveColor = style.PositiveColor;
		NegativeColor = style.NegativeColor;
	}
}
