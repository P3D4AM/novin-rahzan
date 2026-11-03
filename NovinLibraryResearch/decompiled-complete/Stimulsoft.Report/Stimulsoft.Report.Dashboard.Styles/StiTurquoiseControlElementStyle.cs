using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiTurquoiseControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTurquoiseDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Turquoise;

	public override Color SelectedBackColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("2ec6c8", (byte)30);

}
