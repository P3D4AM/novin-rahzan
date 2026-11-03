using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGrayPivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGrayDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGray;

	public override Color CellBackColor { get; set; } = StiColor.Get("595b65");


	public override Color CellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("6b6e75");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color SelectedCellBackColor { get; set; } = StiColor.Get("db7b46");


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("eee");


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("6b6e75");


	public override Color ColumnHeaderForeColor { get; set; } = StiColor.Get("ddd");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("6b6e75");


	public override Color RowHeaderForeColor { get; set; } = StiColor.Get("ddd");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Light("db7b46", (byte)30);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Light("db7b46", (byte)30);


	public override Color LineColor { get; set; } = StiColor.Get("66d3d3d5");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");

}
