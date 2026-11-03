using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSlateGrayControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSlateGrayDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "SlateGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.SlateGray;

	public override Color BackColor { get; set; } = StiColor.Get("33475b");


	public override Color ForeColor { get; set; } = StiColor.Get("ddd");


	public override Color SelectedBackColor { get; set; } = StiColorUtils.Light("33475b", (byte)20);


	public override Color SelectedForeColor { get; set; } = StiColor.Get("eee");


	public override Color GlyphColor { get; set; } = StiColor.Get("ddd");


	public override Color SeparatorColor { get; set; } = StiColor.Get("c0c0c0");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("eee");


	public override Color HotBackColor { get; set; } = StiColorUtils.Dark("33475b", (byte)10);


	public override Color HotForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("eee");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("33475b", (byte)50);


	public override Color HotSelectedForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotSelectedGlyphColor { get; set; } = StiColor.Get("ddd");

}
