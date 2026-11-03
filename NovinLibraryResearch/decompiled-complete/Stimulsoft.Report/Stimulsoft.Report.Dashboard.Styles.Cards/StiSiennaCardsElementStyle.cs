using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles.Cards;

public class StiSiennaCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSiennaDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Sienna;

	public override Color CellBackColor { get; set; } = StiColor.Get("#eee8dc");


	public override Color CellForeColor { get; set; } = StiColor.Get("#270e09");


	public override Color BackColor { get; set; } = StiColor.Get("#595b65");


	public override Color LineColor { get; set; } = StiColor.Get("#c9c2b2");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("#d8834d");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("#8a693c");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("#e83437");


	public override Color CellWinLossPositive { get; set; } = StiColor.Get("#8a693c");


	public override Color CellSparkline { get; set; } = StiColor.Get("#906e3e");


	public override Color CellIndicatorPositive { get; set; } = StiColor.Get("#8a693c");


	public override Color CellIndicatorNegative { get; set; } = StiColor.Get("#e83437");

}
