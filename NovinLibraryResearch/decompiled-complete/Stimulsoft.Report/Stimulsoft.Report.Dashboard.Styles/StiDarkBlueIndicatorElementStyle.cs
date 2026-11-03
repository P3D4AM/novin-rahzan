using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkBlueIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkBlueDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkBlue;

	public override Color GlyphColor { get; set; } = StiColor.Get("165d9e");


	public override Color BackColor { get; set; } = StiColor.Get("0a325a");

}
