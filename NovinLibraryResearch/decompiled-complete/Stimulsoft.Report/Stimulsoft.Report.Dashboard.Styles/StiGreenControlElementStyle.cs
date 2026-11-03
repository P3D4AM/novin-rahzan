using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiGreenControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiGreenDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "Green");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Green;

	public override Color SelectedBackColor { get; set; } = StiColor.Get("70ad47");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("70ad47", (byte)30);

}
