using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiTurquoiseCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTurquoiseDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Turquoise;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color CellSparkline { get; set; } = StiColor.Get("2ec6c8");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("23d9db");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("2ec6c8");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("dd5555");

}
