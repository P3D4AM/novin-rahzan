using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiGreenIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiGreenDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Green;

	public override Color GlyphColor { get; set; } = StiColor.Get("70ad47");

}
