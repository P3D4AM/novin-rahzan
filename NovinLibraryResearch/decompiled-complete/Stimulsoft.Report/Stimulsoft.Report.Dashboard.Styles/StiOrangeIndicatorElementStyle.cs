using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiOrangeIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiOrangeDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Orange;

	public override Color GlyphColor { get; set; } = StiColor.Get("ed7d31");

}
