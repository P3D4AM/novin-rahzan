using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSiennaTableElementStyle : StiTableElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSiennaDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Sienna;

	public override Color CellBackColor { get; set; } = StiColor.Get("#eee8dc");


	public override Color CellForeColor { get; set; } = StiColor.Get("#270e09");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("#e8e0d3");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("#270e09");


	public override Color HeaderBackColor { get; set; } = StiColor.Get("#5c4942");


	public override Color HeaderForeColor { get; set; } = StiColor.Get("#e1dfe0");


	public override Color FooterBackColor { get; set; } = StiColor.Get("#5c4942");


	public override Color FooterForeColor { get; set; } = StiColor.Get("#e1dfe0");


	public override Color SelectedCellBackColor { get; set; } = StiColor.Get("#c0b49c");


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("#270e09");


	public override Color HotHeaderBackColor { get; set; } = StiColor.Get("#5d4942");


	public override Color LineColor { get; set; } = StiColor.Get("#c9c2b2");


	public override Color BackColor { get; set; } = StiColor.Get("#595b65");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("#d8834d");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("#8a693c");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("#e83437");


	public override Color CellWinLossPositive { get; set; } = StiColor.Get("#8a693c");


	public override Color CellSparkline { get; set; } = StiColor.Get("#906e3e");


	public override Color CellIndicatorPositive { get; set; } = StiColor.Get("#8a693c");


	public override Color CellIndicatorNegative { get; set; } = StiColor.Get("#e83437");

}
