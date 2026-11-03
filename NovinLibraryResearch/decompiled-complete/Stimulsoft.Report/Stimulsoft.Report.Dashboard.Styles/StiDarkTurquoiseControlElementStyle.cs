using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkTurquoiseControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkTurquoiseDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkTurquoise;

	public override Color BackColor { get; set; } = StiColor.Get("235e6d");


	public override Color ForeColor { get; set; } = StiColor.Get("fbffff");


	public override Color SelectedBackColor { get; set; } = StiColor.Get("f0621e");


	public override Color SelectedForeColor { get; set; } = StiColor.Get("eee");


	public override Color GlyphColor { get; set; } = StiColor.Get("fbffff");


	public override Color SeparatorColor { get; set; } = StiColor.Get("aed2e5");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("fbffff");


	public override Color HotBackColor { get; set; } = StiColorUtils.Dark("f0621e", (byte)10);


	public override Color HotForeColor { get; set; } = StiColor.Get("fbffff");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("fbffff");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("f0621e", (byte)50);


	public override Color HotSelectedForeColor { get; set; } = StiColor.Get("fbffff");


	public override Color HotSelectedGlyphColor { get; set; } = StiColor.Get("fbffff");

}
