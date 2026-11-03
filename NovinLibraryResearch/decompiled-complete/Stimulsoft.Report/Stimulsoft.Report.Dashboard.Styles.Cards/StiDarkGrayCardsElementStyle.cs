using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles.Cards;

public class StiDarkGrayCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGrayDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGray;

	public override Color CellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color CellBackColor { get; set; } = StiColor.Get("595b65");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");


	public override Color LineColor { get; set; } = StiColor.Get("66d3d3d5");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("db7b46");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("db7b46");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("dd5555");


	public override Color CellWinLossPositive { get; set; } = StiColor.Get("db7b46");


	public override Color CellSparkline { get; set; } = StiColor.Get("db7b46");


	public override Color CellIndicatorPositive { get; set; } = Color.LimeGreen;


	public override Color CellIndicatorNegative { get; set; } = StiColor.Get("dd5555");

}
