using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiOrangeCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiOrangeDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Orange;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color CellSparkline { get; set; } = StiColor.Get("ed7d31");


	public override Color CellDataBarsOverlapped { get; set; } = StiColor.Get("db7b46");


	public override Color CellDataBarsPositive { get; set; } = StiColor.Get("db7b46");


	public override Color CellDataBarsNegative { get; set; } = StiColor.Get("dd5555");

}
