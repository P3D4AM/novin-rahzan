using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSiennaPivotElementStyle : StiPivotElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiAliceBlueDashboardPivotStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Sienna;

	public override Color CellBackColor { get; set; } = StiColor.Get("#eee8dc");


	public override Color CellForeColor { get; set; } = StiColor.Get("#270e09");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("#e8e0d3");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("#270e09");


	public override Color SelectedCellBackColor { get; set; } = StiColor.Get("#c0b49c");


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("#270e09");


	public override Color ColumnHeaderBackColor { get; set; } = StiColor.Get("#5d4942");


	public override Color ColumnHeaderForeColor { get; set; } = StiColor.Get("#eeeeee");


	public override Color RowHeaderBackColor { get; set; } = StiColor.Get("#5d4942");


	public override Color RowHeaderForeColor { get; set; } = StiColor.Get("#eeeeee");


	public override Color HotColumnHeaderBackColor { get; set; } = StiColor.Get("#270e09");


	public override Color HotRowHeaderBackColor { get; set; } = StiColor.Get("#270e09");


	public override Color LineColor { get; set; } = StiColor.Get("#c9c2b2");


	public override Color BackColor { get; set; } = StiColor.Get("#595b65");

}
