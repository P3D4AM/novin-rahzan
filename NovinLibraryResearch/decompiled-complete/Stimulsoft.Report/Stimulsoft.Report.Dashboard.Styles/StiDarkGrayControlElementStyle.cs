using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGrayControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGrayDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGray;

	public override Color BackColor { get; set; } = StiColor.Get("595b65");


	public override Color ForeColor { get; set; } = StiColor.Get("ddd");


	public override Color SelectedBackColor { get; set; } = StiColor.Get("db7b46");


	public override Color SelectedForeColor { get; set; } = StiColor.Get("eee");


	public override Color GlyphColor { get; set; } = StiColor.Get("db7b46");


	public override Color SeparatorColor { get; set; } = StiColor.Get("66d3d3d5");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("eee");


	public override Color HotBackColor { get; set; } = StiColorUtils.Dark("db7b46", (byte)10);


	public override Color HotForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("eee");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("db7b46", (byte)50);


	public override Color HotSelectedForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotSelectedGlyphColor { get; set; } = StiColor.Get("ddd");

}
