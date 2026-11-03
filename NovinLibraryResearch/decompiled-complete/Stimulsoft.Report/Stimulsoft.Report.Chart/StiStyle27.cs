using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStyle27 : StiChartStyle
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "SlateGray");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.SlateGray;

	public override bool IsOffice2015Style => true;

	public override StiChartStyle CreateNew()
	{
		return new StiStyle27();
	}

	public StiStyle27()
	{
		base.Core = new StiStyleCoreXF27();
	}
}
