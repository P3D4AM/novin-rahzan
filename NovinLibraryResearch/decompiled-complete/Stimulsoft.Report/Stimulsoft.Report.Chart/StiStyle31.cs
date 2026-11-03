using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle31 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkTurquoise;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle31();
	}

	public StiStyle31()
	{
		base.Core = new StiStyleCoreXF31();
	}
}
