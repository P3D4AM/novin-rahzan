using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle29 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Blue;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle29();
	}

	public StiStyle29()
	{
		base.Core = new StiStyleCoreXF29();
	}
}
