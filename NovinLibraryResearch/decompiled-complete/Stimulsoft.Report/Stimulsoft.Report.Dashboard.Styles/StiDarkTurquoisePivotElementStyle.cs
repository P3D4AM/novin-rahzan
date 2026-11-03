using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkTurquoisePivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkTurquoiseDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkTurquoise;

	public override Color CellBackColor { get; set; } = StiColor.Get("235e6d");


	public override Color CellForeColor { get; set; } = StiColor.Get("fefdff");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("215667");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("fefdff");


	public override Color SelectedCellBackColor { get; set; } = StiColor.Get("f0621e");


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("1c445b");


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("2a7f9e");


	public override Color ColumnHeaderForeColor { get; set; } = StiColor.Get("fefdff");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("2a7f9e");


	public override Color RowHeaderForeColor { get; set; } = StiColor.Get("fefdff");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColor.Get("f0621e");


	public override Color HotRowHeaderBackColor { get; set; } = StiColor.Get("f0621e");


	public override Color LineColor { get; set; } = StiColor.Get("5da0b7");


	public override Color BackColor { get; set; } = StiColor.Get("235e6d");

}
