using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGreenPivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGreenDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGreen;

	public override Color CellBackColor { get; set; } = StiColor.Get("3f745e");


	public override Color CellForeColor { get; set; } = StiColor.Get("f6f3f5");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("509190");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("e3ba41");


	public override Color SelectedCellBackColor { get; set; } = StiColor.Get("c87f2e");


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("f5f0ea");


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("264945");


	public override Color ColumnHeaderForeColor { get; set; } = StiColor.Get("f2f6f6");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("264945");


	public override Color RowHeaderForeColor { get; set; } = StiColor.Get("f2f6f6");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Light("264945", (byte)30);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Light("264945", (byte)30);


	public override Color LineColor { get; set; } = StiColor.Get("66d3d3d5");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");

}
