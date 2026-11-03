using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSiennaControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSiennaDashboardControlStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Sienna;

	public override Color BackColor { get; set; } = StiColor.Get("#fefefe");


	public override Color ForeColor { get; set; } = StiColor.Get("#1e1e1e");


	public override Color GlyphColor { get; set; } = StiColor.Get("#1e1e1e");


	public override Color SeparatorColor { get; set; } = StiColor.Get("#c9c2b2");


	public override Color SelectedBackColor { get; set; } = StiColor.Get("#c1b59b");


	public override Color SelectedForeColor { get; set; } = StiColor.Get("#170a00");


	public override Color SelectedGlyphColor { get; set; } = StiColor.Get("#170a00");


	public override Color HotBackColor { get; set; } = StiColorUtils.Light(StiColor.Get("#c1b59b"), (byte)30);


	public override Color HotForeColor { get; set; } = StiColor.Get("#170a00");


	public override Color HotGlyphColor { get; set; } = StiColor.Get("#170a00");


	public override Color HotSelectedBackColor { get; set; } = StiColor.Get("#895530");


	public override Color HotSelectedForeColor { get; set; } = StiColor.Get("#e4b16f");


	public override Color HotSelectedGlyphColor { get; set; } = StiColor.Get("#e4b16f");

}
