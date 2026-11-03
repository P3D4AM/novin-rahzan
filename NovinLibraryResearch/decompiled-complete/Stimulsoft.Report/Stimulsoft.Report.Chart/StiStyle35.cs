using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle35 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Sienna;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle35();
	}

	public StiStyle35()
	{
		base.Core = new StiStyleCoreXF35();
	}
}
