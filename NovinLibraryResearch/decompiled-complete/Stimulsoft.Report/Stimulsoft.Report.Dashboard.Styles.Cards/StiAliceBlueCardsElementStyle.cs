using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles.Cards;

public class StiAliceBlueCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiAliceBlueDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "AliceBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.AliceBlue;

	public override Color CellForeColor { get; set; } = StiColor.Get("1e1e1e");


	public override Color CellBackColor { get; set; } = StiColor.Get("f2f5fc");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");


	public override Color LineColor { get; set; } = StiColor.Get("c0cae9");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("40568d");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("40568d");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("d43642");


	public override Color CellWinLossPositive { get; set; } = StiColor.Get("40568d");


	public override Color CellSparkline { get; set; } = StiColor.Get("273946");


	public override Color CellIndicatorPositive { get; set; } = Color.LimeGreen;


	public override Color CellIndicatorNegative { get; set; } = StiColor.Get("dd5555");

}
