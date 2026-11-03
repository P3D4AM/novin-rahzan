using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiOrangeControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiOrangeDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Orange;

	public override Color SelectedBackColor { get; set; } = StiColor.Get("ed7d31");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("ed7d31", (byte)30);

}
