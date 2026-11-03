using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiTurquoisePivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTurquoiseDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Turquoise;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("2ec6c8", (byte)30);


	public override Color SelectedCellForeColor { get; set; } = Color.White;


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Light("2ec6c8", (byte)20);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Light("2ec6c8", (byte)20);

}
