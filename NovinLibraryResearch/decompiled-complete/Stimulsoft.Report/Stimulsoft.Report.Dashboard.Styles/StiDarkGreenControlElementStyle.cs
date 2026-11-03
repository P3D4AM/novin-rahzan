using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGreenControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGreenDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGreen;

	public override Color BackColor { get; set; } = StiColor.Get("3f745e");


	public override Color ForeColor { get; set; } = StiColor.Get("f6f7f7");


	public override Color SelectedBackColor { get; set; } = StiColor.Get("cf822e");


	public override Color SelectedForeColor { get; set; } = StiColor.Get("f6f7f7");


	public override Color GlyphColor { get; set; } = StiColor.Get("e3ba41");


	public override Color SeparatorColor { get; set; } = StiColor.Get("#66d3d3d5");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("f6f7f7");


	public override Color HotBackColor { get; set; } = StiColorUtils.Dark("cf822e", (byte)10);


	public override Color HotForeColor { get; set; } = StiColor.Get("f6f7f7");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("f6f7f7");


	public override Color HotSelectedBackColor { get; set; } = StiColor.Get("c76858");


	public override Color HotSelectedForeColor { get; set; } = StiColor.Get("eeecdd");


	public override Color HotSelectedGlyphColor { get; set; } = StiColor.Get("eeecdd");

}
