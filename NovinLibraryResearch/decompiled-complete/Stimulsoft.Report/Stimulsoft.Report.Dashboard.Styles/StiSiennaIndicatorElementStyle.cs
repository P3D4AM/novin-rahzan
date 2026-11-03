using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSiennaIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSiennaDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Sienna;

	public override Color PositiveColor { get; set; } = StiColor.Get("#8a693c");


	public override Color NegativeColor { get; set; } = StiColor.Get("#e83437");


	public override Color GlyphColor { get; set; } = StiColor.Get("#8b5234");

}
