using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiBlueControlElementStyle : StiControlElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiBlueDashboardControlStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;
}
