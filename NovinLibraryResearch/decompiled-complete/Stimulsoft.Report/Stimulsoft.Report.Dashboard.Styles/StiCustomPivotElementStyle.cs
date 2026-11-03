using System.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiCustomPivotElementStyle : StiPivotElementStyle
{
	private string name;

	public override StiComponentId ComponentId => StiComponentId.StiCustomDashboardPivotStyle;

	public override string LocalizedName => name;

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Custom;

	public override Color CellBackColor { get; set; }

	public override Color AlternatingCellBackColor { get; set; }

	public override Color SelectedCellBackColor { get; set; }

	public override Color SelectedCellForeColor { get; set; }

	public override Color ColumnHeaderBackColor { get; set; }

	public override Color ColumnHeaderForeColor { get; set; }

	public override Color RowHeaderBackColor { get; set; }

	public override Color RowHeaderForeColor { get; set; }

	public override Color HotColumnHeaderBackColor { get; set; }

	public override Color HotRowHeaderBackColor { get; set; }

	public override Color CellForeColor { get; set; }

	public override Color LineColor { get; set; }

	public StiCustomPivotElementStyle(StiCrossTabStyle style)
	{
		name = style.Name;
		BackColor = style.BackColor;
		AlternatingCellForeColor = style.AlternatingCellForeColor;
		AlternatingCellBackColor = style.AlternatingCellBackColor;
		CellBackColor = style.CellBackColor;
		CellForeColor = style.CellForeColor;
		ColumnHeaderBackColor = style.ColumnHeaderBackColor;
		ColumnHeaderForeColor = style.ColumnHeaderForeColor;
		HotColumnHeaderBackColor = style.HotColumnHeaderBackColor;
		HotRowHeaderBackColor = style.HotRowHeaderBackColor;
		RowHeaderBackColor = style.RowHeaderBackColor;
		RowHeaderForeColor = style.RowHeaderForeColor;
		SelectedCellBackColor = style.SelectedCellBackColor;
		SelectedCellForeColor = style.SelectedCellForeColor;
		LineColor = style.LineColor;
	}
}
