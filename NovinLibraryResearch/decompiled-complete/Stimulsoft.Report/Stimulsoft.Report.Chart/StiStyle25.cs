using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle25 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Green;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle25();
	}

	public StiStyle25()
	{
		base.Core = new StiStyleCoreXF25();
	}
}
