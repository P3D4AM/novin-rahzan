using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSilverPivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSilverDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Silver;

	public override Color CellBackColor { get; set; } = StiColor.Get("6d7e8b");


	public override Color CellForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("3a5263");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color SelectedCellBackColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("3a5263");


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("3a5263");


	public override Color ColumnHeaderForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("3a5263");


	public override Color RowHeaderForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Light("6d7e8b", (byte)30);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Light("6d7e8b", (byte)30);


	public override Color LineColor { get; set; } = StiColor.Get("66e9f5fc");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");

}
