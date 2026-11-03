using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle33 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "AliceBlue");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.AliceBlue;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle33();
	}

	public StiStyle33()
	{
		base.Core = new StiStyleCoreXF33();
	}
}
