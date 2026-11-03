using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiOrangePivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiOrangeDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Orange;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("ed7d31", (byte)30);


	public override Color SelectedCellForeColor { get; set; } = Color.White;


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("ed7d31");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("ed7d31");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Light("ed7d31", (byte)20);


	public override Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Light("ed7d31", (byte)20);

}
