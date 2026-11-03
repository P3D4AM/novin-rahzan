using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiBluePivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiBlueDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("3498db", (byte)30);


	public override Color SelectedCellForeColor { get; set; } = Color.White;


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("3498db");


	public override Color ColumnHeaderForeColor { get; set; } = StiColor.Get("fff");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("3498db");


	public override Color RowHeaderForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Dark("3498db", (byte)30);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Dark("3498db", (byte)30);


	public override Color CellForeColor { get; set; } = StiColor.Get("323a45");

}
