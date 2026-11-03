using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle28 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkBlue;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle28();
	}

	public StiStyle28()
	{
		base.Core = new StiStyleCoreXF28();
	}
}
