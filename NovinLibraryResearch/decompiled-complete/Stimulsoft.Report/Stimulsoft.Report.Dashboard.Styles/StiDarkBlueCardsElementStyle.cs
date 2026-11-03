using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkBlueCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkBlueDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkBlue;

	public override Color CellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color CellBackColor { get; set; } = StiColor.Get("1f4265");


	public override Color BackColor { get; set; } = StiColor.Get("0a325a");


	public override Color LineColor => StiColor.Get("758696");
}
