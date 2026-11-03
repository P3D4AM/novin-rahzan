using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles.Cards;

public class StiBlueCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiBlueDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color CellForeColor { get; set; } = StiColor.Get("323a45");

}
