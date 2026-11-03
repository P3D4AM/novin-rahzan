using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSilverControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSilverDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Silver;

	public override Color BackColor { get; set; } = StiColor.Get("6d7e8b");


	public override Color ForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color GlyphColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color SeparatorColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color SelectedBackColor { get; set; } = StiColor.Get("3a5263");


	public override Color SelectedForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color HotBackColor { get; set; } = StiColorUtils.Light("3a5263", (byte)60);


	public override Color HotForeColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("3a5263", (byte)30);


	public override Color HotSelectedForeColor { get; set; } = StiColorUtils.Light("e9f5fc", (byte)30);


	public override Color HotSelectedGlyphColor { get; set; } = StiColorUtils.Light("e9f5fc", (byte)30);

}
