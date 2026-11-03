using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiAliceBlueIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiAliceBlueDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "AliceBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.AliceBlue;

	public override Color GlyphColor { get; set; } = StiColor.Get("40568d");

}
