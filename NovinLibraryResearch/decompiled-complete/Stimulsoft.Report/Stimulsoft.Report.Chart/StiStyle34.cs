using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle34 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => Loc.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkGreen;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle34();
	}

	public StiStyle34()
	{
		base.Core = new StiStyleCoreXF34();
	}
}
