using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiBlueIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiBlueDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public override Color GlyphColor { get; set; } = StiColor.Get("3498db");

}
