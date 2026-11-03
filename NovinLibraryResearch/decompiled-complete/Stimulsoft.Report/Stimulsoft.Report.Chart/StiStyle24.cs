using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle24 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Orange;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle24();
	}

	public StiStyle24()
	{
		base.Core = new StiStyleCoreXF24();
	}
}
