using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle32 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Silver;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle32();
	}

	public StiStyle32()
	{
		base.Core = new StiStyleCoreXF32();
	}
}
