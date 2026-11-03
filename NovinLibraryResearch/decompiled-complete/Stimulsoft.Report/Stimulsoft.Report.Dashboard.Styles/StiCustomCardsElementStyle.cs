namespace Stimulsoft.Report.Dashboard.Styles;

public class StiCustomCardsElementStyle : StiCardsElementStyle
{
	private string name;

	public override StiComponentId ComponentId => StiComponentId.StiCustomDashboardTableStyle;

	public override string LocalizedName => name;

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public StiCustomCardsElementStyle(StiCardsStyle style)
	{
		name = style.Name;
		BackColor = style.BackColor;
		LineColor = style.LineColor;
	}
}
