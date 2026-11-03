using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkBlueControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkBlueDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkBlue;

	public override Color SeparatorColor { get; set; } = StiColor.Get("758696");


	public override Color BackColor { get; set; } = StiColor.Get("0a325a");


	public override Color ForeColor { get; set; } = StiColor.Get("ccc");


	public override Color GlyphColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedBackColor { get; set; } = StiColorUtils.Light("0a325a", (byte)20);


	public override Color SelectedForeColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("eee");


	public override Color HotBackColor { get; set; } = StiColorUtils.Dark("0a325a", (byte)10);


	public override Color HotForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("eee");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("0a325a", (byte)40);


	public override Color HotSelectedForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotSelectedGlyphColor { get; set; } = StiColor.Get("ddd");

}
