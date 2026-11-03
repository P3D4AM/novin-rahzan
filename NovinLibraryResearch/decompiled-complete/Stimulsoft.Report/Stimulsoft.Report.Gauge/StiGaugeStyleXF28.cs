using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF28 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkBlue;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF28();
	}

	public StiGaugeStyleXF28()
	{
		base.Core = new StiGaugeStyleCoreXF28();
	}
}
