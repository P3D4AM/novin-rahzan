using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSilverCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSilverDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Silver;

	public override Color CellForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color CellBackColor { get; set; } = StiColor.Get("6d7e8b");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");


	public override Color LineColor { get; set; } = StiColor.Get("66e9f5fc");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("273946");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("273946");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("d43642");


	public override Color CellWinLossPositive { get; set; } = StiColor.Get("273946");


	public override Color CellSparkline { get; set; } = StiColor.Get("273946");


	public override Color CellIndicatorPositive { get; set; } = Color.LimeGreen;


	public override Color CellIndicatorNegative { get; set; } = StiColor.Get("dd5555");

}
