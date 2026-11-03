using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiAliceBlueControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiAliceBlueDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "AliceBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.AliceBlue;

	public override Color BackColor { get; set; } = StiColor.Get("f2f5fc");


	public override Color ForeColor { get; set; } = StiColor.Get("1e1e1e");


	public override Color GlyphColor { get; set; } = StiColor.Get("1e1e1e");


	public override Color SeparatorColor { get; set; } = StiColor.Get("c0cae9");


	public override Color SelectedBackColor { get; set; } = StiColor.Get("40568d");


	public override Color SelectedForeColor { get; set; } = StiColor.Get("e4ffff");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("e4ffff");


	public override Color HotBackColor { get; set; } = StiColor.Get("f5cc84");


	public override Color HotForeColor { get; set; } = StiColor.Get("1e1e1e");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("1e1e1e");


	public override Color HotSelectedBackColor { get; set; } = StiColor.Get("5d6b99");


	public override Color HotSelectedForeColor { get; set; } = StiColor.Get("e4ffff");


	public override Color HotSelectedGlyphColor { get; set; } = StiColor.Get("e4ffff");

}
