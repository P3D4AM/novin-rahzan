using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiGreenCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiGreenDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Green;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color CellSparkline { get; set; } = StiColor.Get("70ad47");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("74c93a");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("70ad47");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("dd5555");

}
