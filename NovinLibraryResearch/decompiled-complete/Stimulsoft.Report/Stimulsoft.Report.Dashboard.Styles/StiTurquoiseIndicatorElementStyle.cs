using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiTurquoiseIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTurquoiseDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Turquoise;

	public override Color GlyphColor { get; set; } = StiColor.Get("2ec6c8");

}
