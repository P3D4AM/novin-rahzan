using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiGreenPivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiGreenDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Green;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("70ad47", (byte)30);


	public override Color SelectedCellForeColor { get; set; } = Color.White;


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("70ad47");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("70ad47");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Light("70ad47", (byte)20);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Light("70ad47", (byte)20);

}
