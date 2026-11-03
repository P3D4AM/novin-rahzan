using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkTurquoiseCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkTurquoiseDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkTurquoise;

	public override Color CellForeColor { get; set; } = StiColor.Get("fefdff");


	public override Color CellBackColor { get; set; } = StiColor.Get("235e6d");


	public override Color BackColor { get; set; } = StiColor.Get("235e6d");


	public override Color LineColor { get; set; } = StiColor.Get("5da0b7");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("db7b46");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("5daac1");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("dd5555");


	public override Color CellWinLossPositive { get; set; } = StiColor.Get("db7b46");


	public override Color CellSparkline { get; set; } = StiColor.Get("db7b46");


	public override Color CellIndicatorPositive { get; set; } = Color.LimeGreen;


	public override Color CellIndicatorNegative { get; set; } = StiColor.Get("dd5555");

}
