using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF30 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkGray;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF30();
	}

	public StiGaugeStyleXF30()
	{
		base.Core = new StiGaugeStyleCoreXF30();
	}
}
