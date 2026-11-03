using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle30 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkGray;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle30();
	}

	public StiStyle30()
	{
		base.Core = new StiStyleCoreXF30();
	}
}
