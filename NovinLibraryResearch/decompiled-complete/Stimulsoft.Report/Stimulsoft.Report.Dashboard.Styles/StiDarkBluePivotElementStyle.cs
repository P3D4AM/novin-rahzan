using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkBluePivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkBlueDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkBlue;

	public override Color CellBackColor { get; set; } = StiColor.Get("1f4265");


	public override Color CellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("0a325a");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("0a325a", (byte)50);


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("eee");


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("0a325a");


	public override Color ColumnHeaderForeColor { get; set; } = StiColor.Get("ddd");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("0a325a");


	public override Color RowHeaderForeColor { get; set; } = StiColor.Get("ddd");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Light("0a325a", (byte)20);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Light("0a325a", (byte)20);


	public override Color LineColor { get; set; } = StiColor.Get("758696");


	public override Color BackColor { get; set; } = StiColor.Get("0a325a");

}
