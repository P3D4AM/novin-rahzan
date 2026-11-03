namespace Stimulsoft.Report.Dashboard.Styles;

public class StiCustomTableElementStyle : StiTableElementStyle
{
	private string name;

	public override StiComponentId ComponentId => StiComponentId.StiCustomDashboardTableStyle;

	public override string LocalizedName => name;

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public StiCustomTableElementStyle(StiTableStyle style)
	{
		name = style.Name;
		BackColor = style.BackColor;
		AlternatingCellBackColor = style.AlternatingDataColor;
		AlternatingCellForeColor = style.AlternatingDataForeground;
		CellBackColor = style.DataColor;
		CellForeColor = style.DataForeground;
		LineColor = style.GridColor;
		HeaderBackColor = style.HeaderColor;
		HeaderForeColor = style.HeaderForeground;
		HotHeaderBackColor = style.HotHeaderColor;
		SelectedCellBackColor = style.SelectedDataColor;
		SelectedCellForeColor = style.SelectedDataForeground;
		FooterBackColor = style.FooterColor;
		FooterForeColor = style.FooterForeground;
	}
}
