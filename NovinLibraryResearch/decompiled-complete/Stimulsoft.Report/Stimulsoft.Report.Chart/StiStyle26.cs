using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle26 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Turquoise;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle26();
	}

	public StiStyle26()
	{
		base.Core = new StiStyleCoreXF26();
	}
}
